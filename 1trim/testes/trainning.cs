using System.Threading;
using System.Collections;
using System;
class Trainning{
    private List<Word> words = new List<Word>();
    private Random rd = new Random();
    public Trainning(){
        Main();
    }

    private void Main(){
        string f_words = "words.txt";
        build(f_words);
        bool continueIt = true;
        while(continueIt){
            continueIt = trainning();
        }
        debuild(f_words);
    }

    private void println(string s){
        Console.WriteLine(s);
    }
    private void println(float s){
        println(s+"");
    }

    private int getWordSort(){
        float value = (float) rd.NextDouble()*100,ground=0;
        println(value);
        int i = 0;
        foreach (Word word in words){
            if (ground>=value){
                return i;
            }
            ground+= word.prob;
            i++;
        }
        return i-1;
    }

    private bool trainning(){
        Console.Clear();
        int n = getWordSort(),qtOthers=6,ind,addIncorrect = 1;
        int pCorrect = -1;
        Console.WriteLine($"Translation of '{words[n].word}':");
        // forms the options
        List<string> options = new List<string>();
        string correctTranslation = words[n].getOneTranslation();
        options.Add(correctTranslation);
        for (int i = 1; i < qtOthers && i < words[n].pseudo_translations.Count;i++){
            options.Add(words[n].pseudo_translations[i-1]);
        }
        List<Word> others = new List<Word>();
        for (int i = options.Count; i < qtOthers;i++){
            while (true){
                ind = rd.Next(words.Count);
                bool toContinue = false;
                foreach (Word word in others){
                    if (word == words[ind]){
                        toContinue = true;
                        break;
                    }
                }
                if (toContinue) continue;
                others.Add(words[ind]);
                options.Add(words[ind].getOneTranslation());
                break;
            }

        }
        // form the order
        List<string> remain = new List<string>();
        foreach (string word in options){
            remain.Add(word);
        }
        options.Clear();
        for (int i = 0;i<qtOthers;i++){
            ind = rd.Next(qtOthers-i);
            options.Add(remain[ind]);
            remain.RemoveAt(ind);
            if (ind==0 && pCorrect==-1) pCorrect = i;
        }
        // user
        int choice;
        while (true){
            choice = -1;
            ind = 0;
            string[] letters = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z".Split(",");
            foreach (string option in options){
                Console.WriteLine($"{letters[ind]}) '{option}'");
                ind++;
            }
            Console.Write(":> ");
            string answer = Console.ReadLine();
            for (int i=0;i<letters.Length;i++){
                if (letters[i]==answer.ToLower()){
                    choice = i;
                    break;
                }
            }
            if (choice==-1){
                Console.WriteLine("Type one of the letters.");
            }else if (choice==18){ // s
                Console.WriteLine("Saindo ...");
                Thread.Sleep(1000);
                return false;
            }else if (choice>=qtOthers){
                Console.WriteLine("Type one of the letters on the gap.");
            }else{
                break;
            }
        }
        // avaliate
        if (options[choice] == correctTranslation){
            Console.WriteLine("Acertou !");
            ((Word)words[n]).prob = ((Word)words[n]).prob*0.9f;
        }else{
            Console.WriteLine($"Errou ! ('{options[pCorrect]}')");
            ((Word)words[n]).prob += addIncorrect;
            ((Word)words[n]).add_pt((string) options[choice]);
            Thread.Sleep(1000);
        }
        Thread.Sleep(1000);
        return true;
    }

    private void debuild(string f_words){
        string text = "";
        foreach (Word word in words){
            text += $"\"{word.word}\": ";
            int i = 1;
            foreach (string trad in word.translations){
                text += $"\"{trad}\"";
                if (i!=word.translations.Count){
                    text += $",";
                }
                i+=1;
            }
            text += "!";
            i = 1;
            foreach (string p_trad in word.pseudo_translations){
                text += $"\"{p_trad}\"";
                if (i!=word.pseudo_translations.Count){
                    text += $",";
                }
                i+=1;
            }
            text += "|"+word.prob+"\n";

        }
        if (File.Exists(f_words)){
            File.WriteAllText(f_words, text);
        }
    }

    private void build(string f_words){
        if (File.Exists(f_words)){
            Word word = new Word("I");
            foreach (string line in File.ReadAllLines(f_words)) {
                bool collect = false,collectWord=true,collectAgainst=false,getProb=false;
                string mainWord = "",remainWord="";
                foreach (char letter in line) {
                    switch (letter){
                        case '"':
                            collect = (collect) ? false : true;
                            if (!collect && !collectAgainst && !collectWord){
                                word.add_t(remainWord);
                            }else if (!collect && collectAgainst && !collectWord){
                                word.add_pt(remainWord);
                            }
                            if (collect){
                                remainWord = "";
                            }
                            break;
                        case ':':
                            collectWord = false;
                            word = new Word(mainWord);
                            break;
                        case '!':
                            collectAgainst = true;
                            break;
                        case '|':
                            remainWord = "";
                            getProb = true;
                            break;
                        default:
                        
                            if (collect && collectWord){
                                mainWord += letter;
                            }else remainWord+=letter;
                            break;

                    }
                }
                if (float.TryParse(remainWord, out float prob) && getProb){
                    word.prob = prob;
                }else{
                    println(remainWord);
                }
                words.Add(word);
            }
        }else{
            Console.WriteLine("Is not here.");
        }
        float soma = 0;
        foreach (Word word in words){
            soma += word.prob;
        }
        float add = (100-soma)/words.Count;
        println(soma);
        foreach (Word word in words){
            word.prob += add;
        }
        sleep(1);
    }

    public void sleep(float time){
        Thread.Sleep((int) time*1000);
    }


}