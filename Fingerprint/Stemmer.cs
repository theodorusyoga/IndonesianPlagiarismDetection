using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Fingerprint
{
    //NAZIEF-ANDRIANI ALGORITHM
    public class Stemmer
    {
        List<string> dictionary = new List<string>();
        List<string> disallowedCombinations = new List<string>()
        {
            "be-i", "di-an", "ke-i", "ke-kan", "me-an", "se-i", "se-kan"
        };

        List<char> vowels = new List<char>()
        {
            'a', 'i', 'e', 'u', 'o'
        };

        List<string> connection = new List<string>()
        {
            "ng", "m"
        };

        public Stemmer(List<string> dict)
        {
            this.dictionary = dict.ConvertAll(str => str.ToLower());
        }

        internal string PerformNaziefStemming(string word)
        {
            word = word.ToLower();
            if (CheckWordToDictionary(word) || CheckDisallowedSuffixes(word))
                return word;

            word = RemoveInfectionSuffixes(word);
            word = RemoveDerivationSuffixes(word);
            word = RemoveDerivationPrefixes(word);
            return word;
        }

        private bool CheckWordToDictionary(string word)
        {
            string lower = word.ToLower();
            if (this.dictionary.Contains(lower))
                return true;
            else return false;
        }

        private string RemoveInfectionSuffixes(string word)
        {
            word = word.ToLower();
            if (Regex.IsMatch(word, "([km]u|nya|[kl]ah|pun)$"))
                return Regex.Replace(word, "([km]u|nya|[kl]ah|pun)$", "");
            else return word;
        }

        private bool CheckDisallowedSuffixes(string word)
        {
            word = word.ToLower();
            foreach (var item in this.disallowedCombinations)
            {
                string[] split = item.Split('-');
                string regex = "^(" + split[0] + ")[[:alpha:]]+(" + split[1] + ")$";
                if (Regex.IsMatch(word, regex))
                    return true;
            }
            return false;
        }

        private string RemoveDerivationSuffixes(string word)
        {
            word = word.ToLower();
            if (Regex.IsMatch(word, "(i|an|kan)$"))
            {
                string newword = Regex.Replace(word, "(i|an|kan)$", "");
                if (CheckWordToDictionary(newword))
                    return newword;
                else return word;
            }
            else return word;
        }

        private string RemoveDerivationPrefixes(string word)
        {
            word = word.ToLower();
            //prefix di-, ke-, and se-
            if (Regex.IsMatch(word, "^(di|[ks]e)"))
            {
                string newword = Regex.Replace(word, "^(di|[ks]e)", "");
                if (CheckWordToDictionary(newword))
                    return newword;
                string suffixRemoved = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(suffixRemoved))
                    return suffixRemoved;

                //prefix diper- (sub)
                if (Regex.IsMatch(word, "^(diper)"))
                {
                    string _newword = Regex.Replace(word, "^(diper)", "");
                    if (CheckWordToDictionary(_newword))
                        return _newword;
                }
            }

            //prefix te-, me-, be-, pe-, ke-
            if (Regex.IsMatch(word, "^([ktmbp]e)"))
            {
                string newword = Regex.Replace(word, "^([ktmbp]e)", "");
                if (CheckWordToDictionary(newword))
                    return newword;

                if (Regex.IsMatch(newword, "^(ng)"))
                {
                    string _newword = Regex.Replace(newword, "^(ng)", "");
                    if (CheckWordToDictionary(_newword))
                        return _newword;
                }
            }

            if (Regex.IsMatch(word, @"/^([^aiueo])e\\1[aiueo]\S{1,}/i"))
            {
                string newword = Regex.Replace(word, @"/^([^aiueo])e\\1[aiueo]\S{1,}/i", "");
                if (CheckWordToDictionary(newword))
                    return newword;

                string _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
            }

            if (Regex.IsMatch(word, @"/^([tmbp]e)\S{1,}/")) //prefix te-, me-, be-, pe-
            {
                //PREFIX BE-
                if (Regex.IsMatch(word, @"/^(be)\S{1,}/")) //if prefix be-
                {
                    if (Regex.IsMatch(word, @"/^(ber)[aiueo]\S{1,}/"))
                    {
                        string newword = Regex.Replace(word, "/^(ber)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        string __newword = Regex.Replace(word, "/^(ber)/", "r");
                        if (CheckWordToDictionary(__newword))
                            return __newword;
                        string ___newword = RemoveDerivationSuffixes(__newword);
                        if (CheckWordToDictionary(___newword))
                            return ___newword;
                    }

                    if (Regex.IsMatch(word, @"/^(ber)[^aiueor][[:alpha:]](?!er)\S{1,}/"))
                    {
                        string newword = Regex.Replace(word, "/^(ber)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }

                    if (Regex.IsMatch(word, @"/^(ber)[^aiueor][[:alpha:]]er[aiueo]\S{1,}/"))
                    {
                        string newword = Regex.Replace(word, "/^(ber)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }

                    if (Regex.IsMatch(word, @"/^belajar\S{0,}/"))
                    {
                        string newword = Regex.Replace(word, "/^(bel)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }

                    if (Regex.IsMatch(word, @"/^(be)[^aiueolr]er[^aiueo]\S{1,}/"))
                    {
                        string newword = Regex.Replace(word, "/^(be)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                }

                //PREFIX TE-
                if (Regex.IsMatch(word, @"/^(te)\S{1,}/"))
                {
                    if (Regex.IsMatch(word, @"/^(terr)\S{1,}/"))
                        return word;

                    if (Regex.IsMatch(word, @"/^(ter)[aiueo]\S{1,}/")) //rule 6
                    {
                        string newword = Regex.Replace(word, "/^(ter)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        string __newword = Regex.Replace(word, "/^(ter)/", "r");
                        if (CheckWordToDictionary(__newword))
                            return __newword;
                        string ___newword = RemoveDerivationSuffixes(__newword);
                        if (CheckWordToDictionary(___newword))
                            return ___newword;
                    }

                    if (Regex.IsMatch(word, @"/^(ter)[^aiueor]er[aiueo]\S{1,}/")) //rule 7
                    {
                        string newword = Regex.Replace(word, "/^(ter)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }

                    if (Regex.IsMatch(word, @"/^(ter)[^aiueor](?!er)\S{1,}/")) //rule 8
                    {
                        string newword = Regex.Replace(word, "/^(ter)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }

                    if (Regex.IsMatch(word, @"/^(te)[^aiueor]er[aiueo]\S{1,}/")) //rule 9
                    {
                        string newword = Regex.Replace(word, "/^(te)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }

                    if (Regex.IsMatch(word, @"/^(ter)[^aiueor]er[^aiueo]\S{1,}/")) //rule 35
                    {
                        string newword = Regex.Replace(word, "/^(ter)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                }

                //PREFIX ME-
                if (Regex.IsMatch(word, @"/^(me)\S{1,}/"))
                {
                    if (Regex.IsMatch(word, "/^(me)[lrwyv][aiueo]/")) //rule 10
                    {
                        string newword = Regex.Replace(word, "/^(me)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(mem)[bfvp]\S{1,}/")) //rule 11
                    {
                        string newword = Regex.Replace(word, "/^(mem)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(mempe)\S{1,}/")) //rule 12
                    {
                        string newword = Regex.Replace(word, "/^(mem)/", "pe");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(mem)((r[aiueo])|[aiueo])\S{1,}/")) //rule 13
                    {
                        string newword = Regex.Replace(word, "/^(mem)/", "m");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        string __newword = Regex.Replace(word, "/^(mem)/", "p");
                        if (CheckWordToDictionary(__newword))
                            return __newword;
                        string ___newword = RemoveDerivationSuffixes(__newword);
                        if (CheckWordToDictionary(___newword))
                            return ___newword;
                    }
                    if (Regex.IsMatch(word, @"/^(men)[cdjszt]\S{1,}/")) //rule 14
                    {
                        string newword = Regex.Replace(word, "/^(men)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(men)[aiueo]\S{1,}/")) //rule 15
                    {
                        string newword = Regex.Replace(word, "/^(men)/", "n");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        newword = Regex.Replace(word, "/^(men)/", "t");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^ (meng)[ghqk]\S{ 1,}/ ")) //rule 16
                    {
                        string newword = Regex.Replace(word, "/^(meng)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(meng)[aiueo]\S{1,}/")) //rule 17
                    {
                        string newword = Regex.Replace(word, "/^(meng)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        newword = Regex.Replace(word, "/^(meng)/", "k");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        newword = Regex.Replace(word, "/^(menge)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(meny)[aiueo]\S{1,}/")) //rule 18
                    {
                        string newword = Regex.Replace(word, "/^(meny)/", "s");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        newword = Regex.Replace(word, "/^(me)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                }

                //PREFIX PE-
                if (Regex.IsMatch(word, @"/^(pe)\S{1,}/"))
                {
                    if (Regex.IsMatch(word, @"/^(pe)[wy]\S{1,}/")) //rule 20
                    {
                        string newword = Regex.Replace(word, "/^(pe)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(per)[aiueo]\S{1,}/")) //rule 21
                    {
                        string newword = Regex.Replace(word, "/^(per)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        newword = Regex.Replace(word, "/^(per)/", "r");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(per)[^aiueor][[:alpha:]](?!er)\S{1,}/")) //rule 23
                    {
                        string newword = Regex.Replace(word, "/^(per)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(per)[^aiueor][[:alpha:]](er)[aiueo]\S{1,}/")) //rule 24
                    {
                        string newword = Regex.Replace(word, "/^(per)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(pem)[bfv]\S{1,}/")) //rule 25
                    {
                        string newword = Regex.Replace(word, "/^(pem)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(pem)(r[aiueo]|[aiueo])\S{1,}/")) //rule 26
                    {
                        string newword = Regex.Replace(word, "/^(pem)/", "m");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        newword = Regex.Replace(word, "/^(pem)/", "p");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(pen)[cdjzt]\S{1,}/")) //rule 27
                    {
                        string newword = Regex.Replace(word, "/^(pen)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(pen)[aiueo]\S{1,}/")) //rule 28
                    {
                        string newword = Regex.Replace(word, "/^(pen)/", "n");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        newword = Regex.Replace(word, "/^(pen)/", "t");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(peng)[^aiueo]\S{1,}/")) //rule 29
                    {
                        string newword = Regex.Replace(word, "/^(peng)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(peng)[aiueo]\S{1,}/")) //rule 30
                    {
                        string newword = Regex.Replace(word, "/^(peng)/", "n");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        newword = Regex.Replace(word, "/^(peng)/", "k");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        newword = Regex.Replace(word, "/^(penge)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(peny)[aiueo]\S{1,}/")) //rule 31
                    {
                        string newword = Regex.Replace(word, "/^(peny)/", "s");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                        newword = Regex.Replace(word, "/^(pe)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(pel)[aiueo]\S{1,}/")) //rule 32
                    {
                        string newword = Regex.Replace(word, "/^(pel)/", "l");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(pelajar)\S{0,}/"))
                    {
                        string newword = Regex.Replace(word, "/^(pel)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(pe)[^rwylmn]er[aiueo]\S{1,}/")) //rule 33
                    {
                        string newword = Regex.Replace(word, "/^(pe)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(pe)[^rwylmn](?!er)\S{1,}/")) //rule 34
                    {
                        string newword = Regex.Replace(word, "/^(pe)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                    if (Regex.IsMatch(word, @"/^(pe)[^aiueor]er[^aiueo]\S{1,}/")) //rule 36
                    {
                        string newword = Regex.Replace(word, "/^(pe)/", "");
                        if (CheckWordToDictionary(newword))
                            return newword;
                        string _newword = RemoveDerivationSuffixes(newword);
                        if (CheckWordToDictionary(_newword))
                            return _newword;
                    }
                }

                //PREFIX MEMPER-
                if (Regex.IsMatch(word, @"/^(memper)\S{1,}/"))
                {
                    string newword = Regex.Replace(word, "/^(memper)/", "");
                    if (CheckWordToDictionary(newword))
                        return newword;
                    string _newword = RemoveDerivationSuffixes(newword);
                    if (CheckWordToDictionary(_newword))
                        return _newword;
                    newword = Regex.Replace(word, "/^(memper)/", "r");
                    if (CheckWordToDictionary(newword))
                        return newword;
                    _newword = RemoveDerivationSuffixes(newword);
                    if (CheckWordToDictionary(_newword))
                        return _newword;
                }
                //PREFIX MEMPEL-
                if (Regex.IsMatch(word, @"/^(mempel)\S{1,}/"))
                {
                    string newword = Regex.Replace(word, "/^(mempel)/", "");
                    if (CheckWordToDictionary(newword))
                        return newword;
                    string _newword = RemoveDerivationSuffixes(newword);
                    if (CheckWordToDictionary(_newword))
                        return _newword;
                    newword = Regex.Replace(word, "/^(mempel)/", "l");
                    if (CheckWordToDictionary(newword))
                        return newword;
                    _newword = RemoveDerivationSuffixes(newword);
                    if (CheckWordToDictionary(_newword))
                        return _newword;
                }
                //PREFIX MENTER-
                if (Regex.IsMatch(word, @"/^(menter)\S{1,}/"))
                {
                    string newword = Regex.Replace(word, "/^(menter)/", "");
                    if (CheckWordToDictionary(newword))
                        return newword;
                    string _newword = RemoveDerivationSuffixes(newword);
                    if (CheckWordToDictionary(_newword))
                        return _newword;
                    newword = Regex.Replace(word, "/^(menter)/", "r");
                    if (CheckWordToDictionary(newword))
                        return newword;
                    _newword = RemoveDerivationSuffixes(newword);
                    if (CheckWordToDictionary(_newword))
                        return _newword;
                }
                //PREFIX MEMBER-
                if (Regex.IsMatch(word, @"/^(member)\S{1,}/"))
                {
                    string newword = Regex.Replace(word, "/^(member)/", "");
                    if (CheckWordToDictionary(newword))
                        return newword;
                    string _newword = RemoveDerivationSuffixes(newword);
                    if (CheckWordToDictionary(_newword))
                        return _newword;
                    newword = Regex.Replace(word, "/^(member)/", "r");
                    if (CheckWordToDictionary(newword))
                        return newword;
                    _newword = RemoveDerivationSuffixes(newword);
                    if (CheckWordToDictionary(_newword))
                        return _newword;
                }
            }

            //END ME-, BE-, PE-, TE-, KE-

            //PREFIX DIPER-
            if (Regex.IsMatch(word, @"/^(diper)\S{1,}/"))
            {
                string newword = Regex.Replace(word, " /^(diper)/", "");
                if (CheckWordToDictionary(newword))
                    return newword;
                string _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
                newword = Regex.Replace(word, "/^(diper)/", "r");
                if (CheckWordToDictionary(newword))
                    return newword;
                _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
            }

            //PREFIX DITER-
            if (Regex.IsMatch(word, @"/^(diter)\S{1,}/"))
            {
                string newword = Regex.Replace(word, " /^(diter)/", "");
                if (CheckWordToDictionary(newword))
                    return newword;
                string _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
                newword = Regex.Replace(word, "/^(diter)/", "r");
                if (CheckWordToDictionary(newword))
                    return newword;
                _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
            }

            //PREFIX DIPEL-
            if (Regex.IsMatch(word, @"/^(dipel)\S{1,}/"))
            {
                string newword = Regex.Replace(word, " /^(dipel)/", "l");
                if (CheckWordToDictionary(newword))
                    return newword;
                string _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
                newword = Regex.Replace(word, "/^(dipel)/", "");
                if (CheckWordToDictionary(newword))
                    return newword;
                _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
            }

            //PREFIX DIBER-
            if (Regex.IsMatch(word, @"/^(diber)\S{1,}/"))
            {
                string newword = Regex.Replace(word, " /^(diber)/", "");
                if (CheckWordToDictionary(newword))
                    return newword;
                string _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
                newword = Regex.Replace(word, "/^(diber)/", "r");
                if (CheckWordToDictionary(newword))
                    return newword;
                _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
            }

            //PREFIX KEBER-
            if (Regex.IsMatch(word, @"/^(keber)\S{1,}/"))
            {
                string newword = Regex.Replace(word, " /^(keber)/", "");
                if (CheckWordToDictionary(newword))
                    return newword;
                string _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
                newword = Regex.Replace(word, "/^(keber)/", "r");
                if (CheckWordToDictionary(newword))
                    return newword;
                _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
            }

            //PREFIX KETER-
            if (Regex.IsMatch(word, @"/^(keter)\S{1,}/"))
            {
                string newword = Regex.Replace(word, " /^(keter)/", "");
                if (CheckWordToDictionary(newword))
                    return newword;
                string _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
                newword = Regex.Replace(word, "/^(keter)/", "r");
                if (CheckWordToDictionary(newword))
                    return newword;
                _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
            }

            //PREFIX BERKE-
            if (Regex.IsMatch(word, @"/^(berke)\S{1,}/"))
            {
                string newword = Regex.Replace(word, " /^(berke)/", "");
                if (CheckWordToDictionary(newword))
                    return newword;
                string _newword = RemoveDerivationSuffixes(newword);
                if (CheckWordToDictionary(_newword))
                    return _newword;
            }

            if (!Regex.IsMatch(word, @"/^(di|[kstbmp]e)\S{1,}/"))
                return word;

            return word;
        }
    }
}
