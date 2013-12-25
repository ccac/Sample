using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Collections.Generic;
using Microsoft.Windows.Controls;

namespace AutoCompleteBoxSample
{
   public sealed partial class Employee
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

        public Employee()
        {
        }

     
        internal Employee(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        #region Sample data

        public static IEnumerable<Employee> AllExecutives
        {
            get
            {
                yield return new Employee("Walid", "Abu-Hadba");
                yield return new Employee("J", "Allard");
                yield return new Employee("Klaus", "Holse Andersen");
                yield return new Employee("Nancy", "Anderson");
                yield return new Employee("Brian", "Arbogast");
                yield return new Employee("Orlando", "Ayala");
                yield return new Employee("Robert J.", "Bach");
                yield return new Employee("Martha", "Béjar");
                yield return new Employee("Joe", "Belfiore");
                yield return new Employee("Sue", "Bevington");
                yield return new Employee("Frank H.", "Brod");
                yield return new Employee("Brad", "Brooks");
                yield return new Employee("Lisa", "Brummel");
                yield return new Employee("Tom", "Burt");
                yield return new Employee("Chris", "Capossela");
                yield return new Employee("Scott", "Charney");
                yield return new Employee("Debra", "Chrapaty");
                yield return new Employee("Jean-Philippe", "Courtois");
                yield return new Employee("Alain", "Crozier");
                yield return new Employee("Kurt", "DelBene");
                yield return new Employee("Michael", "Delman");
                yield return new Employee("Joe", "DeVaan");
                yield return new Employee("Gerri", "Elliott");
                yield return new Employee("Stephen", "Elop");
                yield return new Employee("Ben", "Fathi");
                yield return new Employee("Grant", "George");
                yield return new Employee("Tom", "Gibbons");
                yield return new Employee("L. Michael", "Golden");
                yield return new Employee("Alexander", "Gounares");
                yield return new Employee("Steve", "Guggenheimer");
                yield return new Employee("Anoop", "Gupta");
                yield return new Employee("Tony", "Hey");
                yield return new Employee("Yasuyuki", "Higuchi");
                yield return new Employee("Roz", "Ho");
                yield return new Employee("Kathleen", "Hogan");
                yield return new Employee("Frank", "Holland");
                yield return new Employee("Todd", "Holmdahl");
                yield return new Employee("Darren", "Huston");
                yield return new Employee("Rajesh", "Jha");
                yield return new Employee("Chris", "Jones");
                yield return new Employee("Erik", "Jorgensen");
                yield return new Employee("Rich", "Kaplan");
                yield return new Employee("Bob", "Kelly");
                yield return new Employee("Jawad", "Khaki");
                yield return new Employee("Shane", "Kim");
                yield return new Employee("Peter", "Klein");
                yield return new Employee("Mitchell L.", "Koch");
                yield return new Employee("Ted", "Kummert");
                yield return new Employee("Julie", "Larson-Green");
                yield return new Employee("Antoine", "Leblond");
                yield return new Employee("Andrew", "Lees");
                yield return new Employee("John M.", "Lervik");
                yield return new Employee("Lewis", "Levin");
                yield return new Employee("Dan'l", "Lewin");
                yield return new Employee("Moshe", "Lichtman");
                yield return new Employee("Christopher", "Liddell");
                yield return new Employee("Steve", "Liffick");
                yield return new Employee("Brian", "MacDonald");
                yield return new Employee("Ron", "Markezich");
                yield return new Employee("Maria", "Martinez");
                yield return new Employee("Mich", "Mathews");
                yield return new Employee("Don A.", "Mattrick");
                yield return new Employee("Joe", "Matz");
                yield return new Employee("Brian", "McAndrews");
                yield return new Employee("Richard", "McAniff");
                yield return new Employee("Yusuf", "Mehdi");
                yield return new Employee("Jim", "Minervino");
                yield return new Employee("William H.", "Mitchell");
                yield return new Employee("Jens Winther", "Moberg");
                yield return new Employee("Mindy", "Mount");
                yield return new Employee("Bob", "Muglia");
                yield return new Employee("Craig", "Mundie");
                yield return new Employee("Terry", "Myerson");
                yield return new Employee("Satya", "Nadella");
                yield return new Employee("Mike", "Nash");
                yield return new Employee("Peter", "Neupert");
                yield return new Employee("Ray", "Ozzie");
                yield return new Employee("Gurdeep", "Singh Pall");
                yield return new Employee("Michael", "Park");
                yield return new Employee("Umberto", "Paolucci");
                yield return new Employee("Sanjay", "Parthasarathy");
                yield return new Employee("Pamela", "Passman");
                yield return new Employee("Alain", "Peracca");
                yield return new Employee("Todd", "Peters");
                yield return new Employee("Joe", "Peterson");
                yield return new Employee("Marshall C.", "Phelps, Jr.");
                yield return new Employee("Scott", "Pitasky");
                yield return new Employee("Will", "Poole");
                yield return new Employee("Rick", "Rashid");
                yield return new Employee("Tami", "Reller");
                yield return new Employee("J.", "Ritchie");
                yield return new Employee("Enrique", "Rodriguez");
                yield return new Employee("Eduardo", "Rosini");
                yield return new Employee("Jon", "Roskill");
                yield return new Employee("Eric", "Rudder");
                yield return new Employee("John", "Schappert");
                yield return new Employee("Tony", "Scott");
                yield return new Employee("Jeanne", "Sheldon");
                yield return new Employee("Harry", "Shum");
                yield return new Employee("Steven", "Sinofsky");
                yield return new Employee("Brad", "Smith");
                yield return new Employee("Mary E.", "Snapp");
                yield return new Employee("Amitabh", "Srivastava");
                yield return new Employee("Kirill", "Tatarinov");
                yield return new Employee("Jeff", "Teper");
                yield return new Employee("David", "Thompson");
                yield return new Employee("Rick", "Thompson");
                yield return new Employee("Brian", "Tobey");
                yield return new Employee("David", "Treadwell");
                yield return new Employee("B. Kevin", "Turner");
                yield return new Employee("David", "Vaskevitch");
                yield return new Employee("Bill", "Veghte");
                yield return new Employee("Henry P.", "Vigil");
                yield return new Employee("Robert", "Wahbe");
                yield return new Employee("Todd", "Warren");
                yield return new Employee("Allison", "Watson");
                yield return new Employee("Blair", "Westlake");
                yield return new Employee("Simon", "Witts");
                yield return new Employee("Robert", "Youngjohns");
                yield return new Employee("Ya-Qin", "Zhang");
                yield return new Employee("George", "Zinn");
            }
        }
        #endregion
    }

    public class SampleEmployeeCollection : ObjectCollection
    {
        public SampleEmployeeCollection() : base(Employee.AllExecutives)
        {  }
    }
}
