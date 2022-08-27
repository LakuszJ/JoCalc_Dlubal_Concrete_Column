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


        ResourceManager rm = new ResourceManager("JCDlubalCSVForcesGeneratorLibrary.Resources.Strings", Assembly.GetExecutingAssembly());

        internal static EndpointAddress Address { get; set; } = new EndpointAddress("http://localhost:8081");

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

        public void MakeConnectionAndGenerateDataFiles()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            LogManager.Configuration = config;
            var logger = LogManager.GetCurrentClassLogger();
            string CurrentDirectory = Directory.GetCurrentDirectory();

            try
            {
                application_information ApplicationInfo;
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
                    ApplicationInfo = application.get_information();
                    logger.Info("Name: {0}, Version:{1}, Type: {2}, language: {3} ", ApplicationInfo.name, ApplicationInfo.version, ApplicationInfo.type, ApplicationInfo.language_name);
                    Console.WriteLine("Name: {0}, Version:{1}, Type: {2}, language: {3} ", ApplicationInfo.name, ApplicationInfo.version, ApplicationInfo.type, ApplicationInfo.language_name);
                }

                switch (ApplicationInfo.language_name)
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
                ModelClient model = new ModelClient(Binding, new EndpointAddress(modelUrl));

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

                model.export_result_tables_with_detailed_members_results_to_csv(CurrentDirectory + @"\CSV_Project_Data");
                //string         gIARM.CSVFilePath = File.ReadAllText(CurrentDirectory + @"\Test.xml" + $"\\{projectName}");
                gIARM.CSVFilePath = CurrentDirectory + @"\CSV_Project_Data" + $"\\{modelName}";
                var fileNames = Directory.GetFiles(gIARM.CSVFilePath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    fileNames[i] = fileNames[i].Substring(fileNames[i].LastIndexOf('\\') + 1);
                }

                gIARM.newCombinationsCSVFileNames = new Dictionary<string, string>();

                foreach (string fileName in fileNames)
                {
                    if (fileName.EndsWith(rm.GetString("static_analysis_members_internal_forces.csv", gIARM.appicationRFEMCuluture)) & fileName.StartsWith(rm.GetString("CO", gIARM.appicationRFEMCuluture)))
                    {
                        string coNo = fileName.Remove(fileName.IndexOf('_'));
                        gIARM.newCombinationsCSVFileNames.Add(coNo, fileName);
                    }
                }

              

                // ANOTHER METHOD THAT SHOUD OPERATE ON "model"
                GetMembersAndCorrectDirection(model);


            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                logger.Error(ex, "Stopped program because of exception :" + ex.Message);
            }
        }

        public void GetMembersAndCorrectDirection(ModelClient model)
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

                mem.memberVector.x = Math.Round(tempBootomNodeForMemberCoordinate.x - tempTopNodeForMemberCoordinate.x, 3);
                mem.memberVector.y = Math.Round(tempBootomNodeForMemberCoordinate.y - tempTopNodeForMemberCoordinate.y, 3);
                mem.memberVector.z = Math.Round(Math.Abs(tempTopNodeForMemberCoordinate.z - tempBootomNodeForMemberCoordinate.z), 3);

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

                members.Add(mem.memberlNo , mem);

            }
        }

        public void CleanCombinationsAtCSVFiles()
        {
            foreach (var file in gIARM.newCombinationsCSVFileNames)
            {
                string tempPath = gIARM.CSVFilePath + "\\" + file.Value;
                string readedText = null;
                if (File.Exists(tempPath))
                {
                    using (StreamReader sr = new StreamReader(tempPath))
                    {
                        var a = sr.BaseStream;

                        readedText = sr.ReadToEnd();

                        string specificChars = GlobalInfomationAboutReadedModel.specificChars;

                        while (readedText.Contains(rm.GetString("Extremes", gIARM.appicationRFEMCuluture)))
                        {
                            readedText = ToolForStringEdition.RemoveStringAreaBetweenPhrases(readedText, rm.GetString("Extremes", gIARM.appicationRFEMCuluture), specificChars);
                        }

                        while (readedText.Contains(rm.GetString("Total max/min values", gIARM.appicationRFEMCuluture)))
                        {
                            readedText = ToolForStringEdition.RemoveStringAreaBetweenPhrases(readedText, (rm.GetString("Total max/min values", gIARM.appicationRFEMCuluture)), "UntilEnd", 0, 1);
                        }

                        readedText = readedText.Remove(0, readedText.IndexOf(rm.GetString("Member Comment", gIARM.appicationRFEMCuluture)) + rm.GetString("Member Comment", gIARM.appicationRFEMCuluture).Length).Trim();
                        readedText = readedText.Remove(readedText.LastIndexOf(specificChars), readedText.Length - readedText.LastIndexOf(specificChars)).Trim();
                        sr.Close();
                    }

                    using (StreamWriter sw = new StreamWriter(tempPath))
                    {
                        sw.Write(readedText);
                        sw.Close();
                    }
                }
            }
        }
    }
}

















