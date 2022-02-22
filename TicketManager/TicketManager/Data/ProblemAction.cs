using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using TicketManager.Models;
using TicketManager.Records;

namespace TicketManager.Data
{
    public class ProblemAction
    {
        public static FirebaseClient firebaseClient =
            new FirebaseClient("https://ticketmanager-16cfb-default-rtdb.europe-west1.firebasedatabase.app/");
        public static async Task GatherAllProblemsAndAnswers()
        {
            var problems = await firebaseClient
                .Child("QA")
                .OnceAsync<object>();

            if (problems.Count > 0)
            {
                ObservableCollection<Problem> rsCollection = new ObservableCollection<Problem>();
                foreach (var item in problems)
                {
                    var json = item.Object.ToString();
                    if (json.Equals("[]"))
                        json = null;


                    if (!string.IsNullOrEmpty(json))
                    {
                        var listProblems = JsonConvert.DeserializeObject<List<Problem>>(json);
                        foreach (var feedback in listProblems)
                        {
                            rsCollection.Add(feedback);
                        }
                    }
                }
                Database.QACollection = rsCollection;
            }
        }

        public static async Task AddProblem(string issue, string solution)
        {
            Problem problem = new Problem
            {
                Issue = issue,
                Solution = solution
            };

            //get data from firebase
            await GatherAllProblemsAndAnswers();
            if (Database.QACollection.All(x => x != problem))
            {
                Database.QACollection.Add(problem);
            }
            
            await firebaseClient.Child("QA").PutAsync(new ProblemRecord
            {
                ProblemsInfoJSON= JsonConvert.SerializeObject(Database.QACollection)

            });
        }
    }
}
