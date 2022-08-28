# JoCalc_Dlubal_Concrete_Column

# Description
This C# projeect is focused on create plugin for Dlubal RFEM 6.xx. 
Original Dlubal RFEM 6.xx calculate concrete columns only with the use nominal curvature method for 2nd order effect at concrete columns.
My goal is the create plugin for calculate with use nominal stiffness method for 2nd order effat and add facilitation for calculate precast elements.
This project is divaide for part:
* Part I 	-  Read and organize data from Dlubal RFEM 6.xx - Mainly it is parsing resulats provaided by Dlubal CSharp library,
* Part II 	- Create core for calculation 2nd order effet (nominal siffnes method),
* Part III 	- Create some simple UI at WPF MVVM,
* Part IV 	- ...

Progress status:
* Part I - Complete > 90%
lack of handling popular exceptions.
** Part II - In progress 
No release

#Gtting started

You should download actual release of C# code, after that You can compile library `JCDlubalCSVForcesGeneratorLibrary.dll` and use it.
You can also test library and use `JoCalcDlubalClinectConsoleUI`.

Before you start remember about:
* You should have RFEM 6.xx enabled and calculated,
* Remember about don't open any additional sub-windows at application RFEM,
* Read missing dependencies,
* Because of saving obj to `.json` your antivirus can activate (You can ofcourse delete method with saving to `.json` or ignore warnings).

 
#What you will get
* `Dictionary<int, Dictionary<int, List<InternalForcesMemberSingleItem>>>`
Whre first `<TKey>` is number of the member at RFEM current model, second `<TKey>` is number of the combination. After that you will get `List<InternalForcesMemberSingleItem>`.

Examble:
```
CSVEditorAndForceGenerator fg = new CSVEditorAndForceGenerator(openAndReadModel.gIARM);
Dictionary<int, Dictionary<int, List<InternalForcesMemberSingleItem>>> dicMCF = fg.GenereteDicOfForcesForAllMembers();
List<InternalForcesMemberSingleItem> listOfForces_3_2 = dicMCF[3][2];
```
as below you will get  'List<InternalForcesMemberSingleItem>' for member no 3 and combination no 5.
* `public class MyMember`
Class with some specific information about members (will be used at Part II).
* `public class GlobalInfomationAboutReadedModel`
Class with information about specification of readed file (ex. CultureInfo with information about name of CSV files). 

Examble of xample of `.json`:
  
![image](https://user-images.githubusercontent.com/97826529/187091086-9f23b161-3a5b-4647-bf22-16835ce0a08b.png)

