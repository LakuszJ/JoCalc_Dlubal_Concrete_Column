using Dlubal.WS.Rfem6.Application;
using ApplicationClient = Dlubal.WS.Rfem6.Application.RfemApplicationClient;
using Dlubal.WS.Rfem6.Model;
using ModelClient = Dlubal.WS.Rfem6.Model.RfemModelClient;

using NLog;
using System.ServiceModel;
using System.Xml.Linq;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Net;

using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Security.Permissions;
using Newtonsoft.Json;

namespace JCDlubalCSVForcesGeneratorLibrary
{
    public class OpenAndReadModel
    {
        public GlobalInfomationAboutReadedModel gIARM = new GlobalInfomationAboutReadedModel();
        public Dictionary<int, MyMember> members = new Dictionary<int, MyMember>();
        public Dictionary<int, Tuple<DesignSituation, string>> dicCombinations = new Dictionary<int, Tuple<DesignSituation, string>>();

        ResourceManager rm = new ResourceManager("JCDlubalCSVForcesGeneratorLibrary.Resources.Strings", Assembly.GetExecutingAssembly());

        internal static EndpointAddress Address { get; set; } = new EndpointAddress("http://localhost:8081");

        string CurrentDirectory = Directory.GetCurrentDirectory();


        internal static BasicHttpBinding Binding
        {
            get
            {
                BasicHttpBinding binding = new BasicHttpBinding
                {
                    // Send timeout is set to 180 seconds.
                    SendTimeout = new TimeSpan(0, 0, 180),
                    UseDefaultWebProxy = true,
                };

                return binding;
            }
        }

        internal static ApplicationClient application;

        public void SetConnectionAndGetDataFiles()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            LogManager.Configuration = config;
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                application_information applicationInfo;
                try
                {
                    application = new ApplicationClient(Binding, Address);

                }
                catch (Exception exception)
                {
                    if (application != null)
                    {
                        if (application.State != CommunicationState.Faulted)
                        {
                            application.Close();
                            logger.Error(exception, "Something happen:" + exception.Message);
                        }
                        else
                        {
                            application.Abort();
                            logger.Error(exception, "Communication with RFEM faulted:" + exception.Message);
                        }
                        application = null;
                    }
                }
                finally
                {
                    applicationInfo = application.get_information();
                    logger.Info("Name: {0}, Version:{1}, Type: {2}, language: {3} ", applicationInfo.name, applicationInfo.version, applicationInfo.type, applicationInfo.language_name);
                    Console.WriteLine("Name: {0}, Version:{1}, Type: {2}, language: {3} ", applicationInfo.name, applicationInfo.version, applicationInfo.type, applicationInfo.language_name);
                }




                string modelUrl = application.get_active_model();
                ModelClient model = new ModelClient(Binding, new EndpointAddress(modelUrl));

                if (Directory.Exists(CurrentDirectory + @"\CSV_Project_Data"))
                {
                    Directory.Delete(CurrentDirectory + @"\CSV_Project_Data", true);
                }
                model.export_result_tables_with_detailed_members_results_to_csv(CurrentDirectory + @"\CSV_Project_Data");

                //model.export_result_tables_to_xml(CurrentDirectory + @"\XML_Project_Data");


                //ANOTHER METHOD THAT SHOUD OPERATE ON "model"
                SetGIARM();
                GetMembersAndCorrectDirection(model);
                GetCombinationList(model);


            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                logger.Error(ex, "Stopped program because of exception :" + ex.Message);
            }
        }

        public void GetCombinationList(ModelClient model)
        {
            foreach (var item in gIARM.combinationsCSVFileNames)
            {
                int coNo = int.Parse(item.Key.Remove(0, 2));
                DesignSituation tempCombinationDesignSituation = (DesignSituation)model.get_load_combination(coNo).design_situation;
                string tempcCombinationName = model.get_action_combination(coNo).name;
                Tuple<DesignSituation, string> tuple = new Tuple<DesignSituation, string>(tempCombinationDesignSituation, tempcCombinationName);
                dicCombinations.Add(coNo, tuple);
            }
        }

        private void SetGIARM()
        {

            application_information applicationInfo = application.get_information();

            switch (applicationInfo.language_name)
            {
                case "English":
                    gIARM.appicationRFEMCuluture = new CultureInfo("en-GB");
                    break;
                case "Polski":
                    gIARM.appicationRFEMCuluture = new CultureInfo("pl-PL");
                    break;
                case "Deutsch":
                    gIARM.appicationRFEMCuluture = new CultureInfo("de-DE");
                    break;
            }

            gIARM.readTime = DateTime.Now;

            string modelUrl = application.get_active_model();
            string[] modelNames = application.get_model_list();
            string modelName = "";
            for (int i = 0; i < modelNames.Length; i++)
            {
                string tempModelUrl = application.get_model(i);
                if (tempModelUrl == modelUrl)
                {
                    modelName = modelNames[i];
                    break;
                }
            }

            gIARM.pathCSVFiles = CurrentDirectory + @"\CSV_Project_Data" + $"\\{modelName}";
            var fileNames = Directory.GetFiles(gIARM.pathCSVFiles);
            for (int i = 0; i < fileNames.Length; i++)
            {
                fileNames[i] = fileNames[i].Substring(fileNames[i].LastIndexOf('\\') + 1);
            }

            gIARM.combinationsCSVFileNames = new Dictionary<string, string>();

            foreach (string fileName in fileNames)
            {
                if (fileName.EndsWith(rm.GetString("static_analysis_members_internal_forces.csv", gIARM.appicationRFEMCuluture)) & fileName.StartsWith(rm.GetString("CO", gIARM.appicationRFEMCuluture)))
                {
                    string coNo = fileName.Remove(fileName.IndexOf('_'));
                    gIARM.combinationsCSVFileNames.Add(coNo, fileName);
                }
            }

        }

        private void GetMembersAndCorrectDirection(ModelClient model)
        {
            int[] allObjectsNumber = model.get_all_object_numbers(object_types.E_OBJECT_TYPE_MEMBER, 1);

            // GLOBAL GOORDINATION SYSTEM Z DIRECTION
            double globalZModelDirection;
            if (model.get_model_settings_and_options().global_axes_orientation.ToString().EndsWith("DOWN"))
            {
                globalZModelDirection = -1;
            }
            else // "UP"
            {
                globalZModelDirection = 1;
            }

            for (int i = 0; i < allObjectsNumber.Length; i++)
            {
                MyMember mem = new MyMember();
                // GETING
                member tempMember = model.get_member(allObjectsNumber[i]); // example - change for loop

                mem.memberlNo = tempMember.no;
                mem.startNode = tempMember.node_start;
                mem.endNode = tempMember.node_end;
                vector_3d tempStartNodeForMemberCoordinate = model.get_node(mem.startNode).global_coordinates;
                vector_3d tempEndNodeForMemberCoordinate = model.get_node(mem.endNode).global_coordinates;

                vector_3d tempTopNodeForMemberCoordinate;
                vector_3d tempBootomNodeForMemberCoordinate;


                if (globalZModelDirection == 1) //it mean that higher value is top node
                {
                    if (tempStartNodeForMemberCoordinate.z > tempEndNodeForMemberCoordinate.z)
                    {
                        mem.topNode = mem.startNode;
                        mem.bottomNode = mem.endNode;
                        tempTopNodeForMemberCoordinate = tempStartNodeForMemberCoordinate;
                        tempBootomNodeForMemberCoordinate = tempEndNodeForMemberCoordinate;

                    }
                    else
                    {
                        mem.topNode = mem.endNode;
                        mem.bottomNode = mem.startNode;
                        tempTopNodeForMemberCoordinate = tempEndNodeForMemberCoordinate;
                        tempBootomNodeForMemberCoordinate = tempStartNodeForMemberCoordinate;
                    }

                }
                else // globalZModelDirection == -1 // it mean that lower value is top node
                {
                    if (tempStartNodeForMemberCoordinate.z < tempEndNodeForMemberCoordinate.z)
                    {
                        mem.topNode = mem.startNode;
                        mem.bottomNode = mem.endNode;
                        tempTopNodeForMemberCoordinate = tempStartNodeForMemberCoordinate;
                        tempBootomNodeForMemberCoordinate = tempEndNodeForMemberCoordinate;

                    }
                    else
                    {
                        mem.topNode = mem.endNode;
                        mem.bottomNode = mem.startNode;
                        tempTopNodeForMemberCoordinate = tempEndNodeForMemberCoordinate;
                        tempBootomNodeForMemberCoordinate = tempStartNodeForMemberCoordinate;
                    }

                }

                mem.memberVectorX = Math.Round(tempBootomNodeForMemberCoordinate.x - tempTopNodeForMemberCoordinate.x, 3);
                mem.memberVectorY= Math.Round(tempBootomNodeForMemberCoordinate.y - tempTopNodeForMemberCoordinate.y, 3);
                mem.memberVectorZ = Math.Round(Math.Abs(tempTopNodeForMemberCoordinate.z - tempBootomNodeForMemberCoordinate.z), 3);

                mem.memberLength = (decimal)Math.Round(tempMember.length, 3);

                mem.memberRotation = tempMember.rotation_angle;

                if (tempMember.section_distribution_type == member_section_distribution_type.SECTION_DISTRIBUTION_TYPE_UNIFORM)
                {
                    int se = tempMember.section_start;
                    section tempSection = model.get_section(allObjectsNumber[se - 1]);  // here shoud be se no se-1 but is bug in DlubalLib --> could be repared at next upd
                    section_type type = tempSection.type;
                    string tempNameSection = tempSection.name;
                    mem.membersSectionType = tempSection.parametrization_type;
                    mem.memberSectionDim = tempNameSection.Substring(tempNameSection.IndexOf(' ')).Trim();
                    mem.sectionRotation = tempSection.rotation_angle;
                }
                else
                {
                    string error = "Lack of section defintion for member"; // some exception
                }

                members.Add(mem.memberlNo, mem);

            }
        }


    }
}

















