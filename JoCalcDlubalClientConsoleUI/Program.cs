using JCDlubalCSVForcesGeneratorLibrary;
using Newtonsoft.Json;

/////////////////////////////////////////////////////////////////////////////////////////

string pathToSaveDirecotry = Directory.GetCurrentDirectory() + @"\JoCalc_ReadedFiles";
if (!Directory.Exists(pathToSaveDirecotry))
{
    Directory.CreateDirectory(pathToSaveDirecotry);
}

OpenAndReadModel openAndReadModel = new OpenAndReadModel();
openAndReadModel.MakeConnectionAndGenerateDataFiles();
openAndReadModel.CleanCombinationsAtCSVFiles();


CSVEditorAndForceGenerator fg = new CSVEditorAndForceGenerator(openAndReadModel.gIARM);
// complete Dictionary of Key: Member --> Key: Combination --> List of single items ( Internal forces at member )
// completeDictionaryOfMemberCombinationForces ---> dicMCF
Dictionary<int, Dictionary<int, List<InternalForcesMemberSingleItem>>> dicMCF = fg.GenereteDicOfForcesForAllMembers(); 



string dicMCFjson = JsonConvert.SerializeObject(dicMCF);
ToolForManipulationFiles.SaveTxtFile(pathToSaveDirecotry+ @"\MembersCombinationsForces.json", dicMCFjson);

// for future mainupation with dicMCF will be neded some addtional info located abot model.
// I woudl like operate on dicMCF without repeating connection to model.

string gIARMjson = JsonConvert.SerializeObject(openAndReadModel.gIARM);
ToolForManipulationFiles.SaveTxtFile(pathToSaveDirecotry + @"\InformationAboutReadedModel.json", gIARMjson);

string membersJson = JsonConvert.SerializeObject(openAndReadModel.members);
ToolForManipulationFiles.SaveTxtFile(pathToSaveDirecotry + @"\MembersInformations.json", membersJson);

System.Diagnostics.Process.Start("explorer.exe", pathToSaveDirecotry);
