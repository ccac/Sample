using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;


namespace AutoCompleteBox.Web.Service
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AutoCompletedService
    {
        [OperationContract]
        public List<EmployeeInfo> GetEmployeeCollection(string name)
        {
            List<EmployeeInfo> EmpolyeeList = new List<EmployeeInfo>();
            foreach (EmployeeInfo ei in (from employeeinfo in EmployeeInfo.AllExecutives
                                         where employeeinfo.DisplayName.StartsWith(name)
                                         select employeeinfo))
            {
                EmpolyeeList.Add(ei);
            }
            return EmpolyeeList;
        }
    }

  
    public sealed partial class EmployeeInfo
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public EmployeeInfo()
        {
        }

      
        internal EmployeeInfo(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        #region Sample data

        public static IEnumerable<EmployeeInfo> AllExecutives
        {
            get
            {
                yield return new EmployeeInfo("Walid", "Abu-Hadba");
                yield return new EmployeeInfo("J", "Allard");
                yield return new EmployeeInfo("Klaus", "Holse Andersen");
                yield return new EmployeeInfo("Nancy", "Anderson");
                yield return new EmployeeInfo("Brian", "Arbogast");
                yield return new EmployeeInfo("Orlando", "Ayala");
                yield return new EmployeeInfo("Robert J.", "Bach");
                yield return new EmployeeInfo("Martha", "Béjar");
                yield return new EmployeeInfo("Joe", "Belfiore");
                yield return new EmployeeInfo("Sue", "Bevington");
                yield return new EmployeeInfo("Frank H.", "Brod");
                yield return new EmployeeInfo("Brad", "Brooks");
                yield return new EmployeeInfo("Lisa", "Brummel");
                yield return new EmployeeInfo("Tom", "Burt");
                yield return new EmployeeInfo("Chris", "Capossela");
                yield return new EmployeeInfo("Scott", "Charney");
                yield return new EmployeeInfo("Debra", "Chrapaty");
                yield return new EmployeeInfo("Jean-Philippe", "Courtois");
                yield return new EmployeeInfo("Alain", "Crozier");
                yield return new EmployeeInfo("Kurt", "DelBene");
                yield return new EmployeeInfo("Michael", "Delman");
                yield return new EmployeeInfo("Joe", "DeVaan");
                yield return new EmployeeInfo("Gerri", "Elliott");
                yield return new EmployeeInfo("Stephen", "Elop");
                yield return new EmployeeInfo("Ben", "Fathi");
                yield return new EmployeeInfo("Grant", "George");
                yield return new EmployeeInfo("Tom", "Gibbons");
                yield return new EmployeeInfo("L. Michael", "Golden");
                yield return new EmployeeInfo("Alexander", "Gounares");
                yield return new EmployeeInfo("Steve", "Guggenheimer");
                yield return new EmployeeInfo("Anoop", "Gupta");
                yield return new EmployeeInfo("Tony", "Hey");
                yield return new EmployeeInfo("Yasuyuki", "Higuchi");
                yield return new EmployeeInfo("Roz", "Ho");
                yield return new EmployeeInfo("Kathleen", "Hogan");
                yield return new EmployeeInfo("Frank", "Holland");
                yield return new EmployeeInfo("Todd", "Holmdahl");
                yield return new EmployeeInfo("Darren", "Huston");
                yield return new EmployeeInfo("Rajesh", "Jha");
                yield return new EmployeeInfo("Chris", "Jones");
                yield return new EmployeeInfo("Erik", "Jorgensen");
                yield return new EmployeeInfo("Rich", "Kaplan");
                yield return new EmployeeInfo("Bob", "Kelly");
                yield return new EmployeeInfo("Jawad", "Khaki");
                yield return new EmployeeInfo("Shane", "Kim");
                yield return new EmployeeInfo("Peter", "Klein");
                yield return new EmployeeInfo("Mitchell L.", "Koch");
                yield return new EmployeeInfo("Ted", "Kummert");
                yield return new EmployeeInfo("Julie", "Larson-Green");
                yield return new EmployeeInfo("Antoine", "Leblond");
                yield return new EmployeeInfo("Andrew", "Lees");
                yield return new EmployeeInfo("John M.", "Lervik");
                yield return new EmployeeInfo("Lewis", "Levin");
                yield return new EmployeeInfo("Dan'l", "Lewin");
                yield return new EmployeeInfo("Moshe", "Lichtman");
                yield return new EmployeeInfo("Christopher", "Liddell");
                yield return new EmployeeInfo("Steve", "Liffick");
                yield return new EmployeeInfo("Brian", "MacDonald");
                yield return new EmployeeInfo("Ron", "Markezich");
                yield return new EmployeeInfo("Maria", "Martinez");
                yield return new EmployeeInfo("Mich", "Mathews");
                yield return new EmployeeInfo("Don A.", "Mattrick");
                yield return new EmployeeInfo("Joe", "Matz");
                yield return new EmployeeInfo("Brian", "McAndrews");
                yield return new EmployeeInfo("Richard", "McAniff");
                yield return new EmployeeInfo("Yusuf", "Mehdi");
                yield return new EmployeeInfo("Jim", "Minervino");
                yield return new EmployeeInfo("William H.", "Mitchell");
                yield return new EmployeeInfo("Jens Winther", "Moberg");
                yield return new EmployeeInfo("Mindy", "Mount");
                yield return new EmployeeInfo("Bob", "Muglia");
                yield return new EmployeeInfo("Craig", "Mundie");
                yield return new EmployeeInfo("Terry", "Myerson");
                yield return new EmployeeInfo("Satya", "Nadella");
                yield return new EmployeeInfo("Mike", "Nash");
                yield return new EmployeeInfo("Peter", "Neupert");
                yield return new EmployeeInfo("Ray", "Ozzie");
                yield return new EmployeeInfo("Gurdeep", "Singh Pall");
                yield return new EmployeeInfo("Michael", "Park");
                yield return new EmployeeInfo("Umberto", "Paolucci");
                yield return new EmployeeInfo("Sanjay", "Parthasarathy");
                yield return new EmployeeInfo("Pamela", "Passman");
                yield return new EmployeeInfo("Alain", "Peracca");
                yield return new EmployeeInfo("Todd", "Peters");
                yield return new EmployeeInfo("Joe", "Peterson");
                yield return new EmployeeInfo("Marshall C.", "Phelps, Jr.");
                yield return new EmployeeInfo("Scott", "Pitasky");
                yield return new EmployeeInfo("Will", "Poole");
                yield return new EmployeeInfo("Rick", "Rashid");
                yield return new EmployeeInfo("Tami", "Reller");
                yield return new EmployeeInfo("J.", "Ritchie");
                yield return new EmployeeInfo("Enrique", "Rodriguez");
                yield return new EmployeeInfo("Eduardo", "Rosini");
                yield return new EmployeeInfo("Jon", "Roskill");
                yield return new EmployeeInfo("Eric", "Rudder");
                yield return new EmployeeInfo("John", "Schappert");
                yield return new EmployeeInfo("Tony", "Scott");
                yield return new EmployeeInfo("Jeanne", "Sheldon");
                yield return new EmployeeInfo("Harry", "Shum");
                yield return new EmployeeInfo("Steven", "Sinofsky");
                yield return new EmployeeInfo("Brad", "Smith");
                yield return new EmployeeInfo("Mary E.", "Snapp");
                yield return new EmployeeInfo("Amitabh", "Srivastava");
                yield return new EmployeeInfo("Kirill", "Tatarinov");
                yield return new EmployeeInfo("Jeff", "Teper");
                yield return new EmployeeInfo("David", "Thompson");
                yield return new EmployeeInfo("Rick", "Thompson");
                yield return new EmployeeInfo("Brian", "Tobey");
                yield return new EmployeeInfo("David", "Treadwell");
                yield return new EmployeeInfo("B. Kevin", "Turner");
                yield return new EmployeeInfo("David", "Vaskevitch");
                yield return new EmployeeInfo("Bill", "Veghte");
                yield return new EmployeeInfo("Henry P.", "Vigil");
                yield return new EmployeeInfo("Robert", "Wahbe");
                yield return new EmployeeInfo("Todd", "Warren");
                yield return new EmployeeInfo("Allison", "Watson");
                yield return new EmployeeInfo("Blair", "Westlake");
                yield return new EmployeeInfo("Simon", "Witts");
                yield return new EmployeeInfo("Robert", "Youngjohns");
                yield return new EmployeeInfo("Ya-Qin", "Zhang");
                yield return new EmployeeInfo("George", "Zinn");
            }
        }
        #endregion
    }
}
