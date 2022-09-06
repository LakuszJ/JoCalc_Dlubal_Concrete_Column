using JCDlubalCSVForcesGeneratorLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


///

string pathToSaveDirecotry = Directory.GetCurrentDirectory() + @"\JoCalc_ReadedFiles";
if (!Directory.Exists(pathToSaveDirecotry))
{
    Directory.CreateDirectory(pathToSaveDirecotry);
}

OpenAndReadModel openAndReadModel = new OpenAndReadModel();
openAndReadModel.SetConnectionAndGetDataFiles();

CSVEditorAndForceGenerator fg = new CSVEditorAndForceGenerator(openAndReadModel.gIARM);
fg.CleanCombinationsFilesAtCSVFiles();
// complete Dictionary of Key: Member --> Key: Combination --> List of single items ( Internal forces at member )
// completeDictionaryOfMemberCombinationForces ---> dicMCF
Dictionary<int, Dictionary<int, List<InternalForcesMemberSingleItem>>> dicMCF = fg.GenereteDicOfForcesForAllMembers();

//Reveare List of Forces if first node IS NOT bottom node
// Finally bottom node always will by first node at results.
ForcesModyficator.ModifyNodeOrder(dicMCF, openAndReadModel.members);

string dicMCFjson = JsonConvert.SerializeObject(dicMCF);
ToolForManipulationFiles.SaveTxtFile(pathToSaveDirecotry + @"\MembersCombinationsForces.json", dicMCFjson);

// for future mainupation with dicMCF will be neded some addtional info located abot model.
// I woudl like operate on dicMCF without repeating connection to model.

string gIARMjson = JsonConvert.SerializeObject(openAndReadModel.gIARM);
ToolForManipulationFiles.SaveTxtFile(pathToSaveDirecotry + @"\InformationAboutReadedModel.json", gIARMjson);

string membersJson = JsonConvert.SerializeObject(openAndReadModel.members);
ToolForManipulationFiles.SaveTxtFile(pathToSaveDirecotry + @"\MembersInformations.json", membersJson);

string combinationJson = JsonConvert.SerializeObject(openAndReadModel.dicCombinations);
ToolForManipulationFiles.SaveTxtFile(pathToSaveDirecotry + @"\Combinations.json", combinationJson);

System.Diagnostics.Process.Start("explorer.exe", pathToSaveDirecotry);


// EXAMPLE OF MULTI LEVEL JSON DESERIALIZAOTR

string jsonStringMCF = ToolForManipulationFiles.ReadTxtFile(pathToSaveDirecotry + @"\MembersCombinationsForces.json");
string jsonStringMemInfo = ToolForManipulationFiles.ReadTxtFile(pathToSaveDirecotry + @"\MembersInformations.json");

var dicMem = ToolForManipulationFiles.deserializeOneLevelDic<MyMember>(jsonStringMemInfo);
var dicReadMCF = ToolForManipulationFiles.deserializeTwoLevelDic<List<InternalForcesMemberSingleItem>>(jsonStringMCF);


Console.ReadKey();

