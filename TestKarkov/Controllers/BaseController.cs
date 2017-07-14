using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TestKarkov.Models;

namespace TestKarkov.Controllers
{
    public class BaseController : Controller
    {
        public const string rulesFile = "~/Data/base.json";
        public const string rulesOrderFile = "~/Data/values.json";
        public const string symbolStringFile = "~/Data/cypher.json";
        public const string wordsFile = "~/Data/words.json";

        protected List<T> JsonFile<T>(string nameFile)
        {
            // read JSON directly from a file
            var path = Server.MapPath(nameFile);
            string Json = System.IO.File.ReadAllText(path);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var personlist = ser.Deserialize<List<T>>(Json);
            return personlist;
        }

        protected List<List<char>> ConvertToArray(List<string> list)
        {
            var newList = new List<List<char>>();
            list.ForEach(x => { newList.Add(x.ToCharArray().ToList());});
            return newList;

        }

        protected List<string> ApplyMarkov()
        {

            var rules = JsonFile<Rule>(rulesFile);
            var rulesOrder = JsonFile<List<RuleOrder>>(rulesOrderFile);

            List<List<RuleOrder>> rigthRulesOrder = new List<List<RuleOrder>>();
            rulesOrder.ForEach(x =>
            {
                rigthRulesOrder.Add(x.OrderBy(d => d.order).ToList());
            });
            var symbolString = JsonFile<string>(symbolStringFile);

            string newWord;
            bool applyNextRule;
            int pivoteRule;
            Rule currentRule;
            Regex expressionSearched;

            //var which say if any rule was applied
            bool appliedRule;
            for (int x = 0; x < symbolString.Count; x++)
            {
                newWord = symbolString[x];
                applyNextRule = true;
                while (applyNextRule)
                {
                    appliedRule = false;
                    pivoteRule = 0;
                    while (pivoteRule < rigthRulesOrder[x].Count())
                    {
                        currentRule = rules[rigthRulesOrder[x][pivoteRule].rule];
                        expressionSearched = new Regex(Regex.Escape(currentRule.source));
                        if (expressionSearched.IsMatch(newWord))
                        {
                            appliedRule = true;
                            newWord = expressionSearched.Replace(newWord, currentRule.replacement, 1);
                        }
                        else if (rigthRulesOrder[x][pivoteRule].isTermination || (pivoteRule + 1 == rigthRulesOrder[x].Count()) && !appliedRule)
                        {
                            applyNextRule = false;
                        }
                        pivoteRule = rigthRulesOrder[x][pivoteRule].isTermination ? rigthRulesOrder[x].Count() : pivoteRule + 1;
                    }

                }
                symbolString[x] = newWord;
            }

            return symbolString;
        }
    }
}