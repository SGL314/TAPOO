using System.Collections;

class Word{
    public string word;
    private Random rd = new Random();
    public List<string> translations = new List<string>();
    public List<string> pseudo_translations = new List<string>();
    public float prob = 0;
    public Word(string word){
        this.word = word;
    }

    public void add_t(string add){
        translations.Add(add);
    }
    public void remove_t(string remove){
        translations.Remove(remove);
    }
    public void add_pt(string add){
        pseudo_translations.Add(add);
    }
    public void remove_pt(string remove){
        pseudo_translations.Remove(remove);
    }
    public string getOneTranslation(){
        return (string) translations[rd.Next(translations.Count)];
    }

    public string show(){
        string init = $"\"{word}\": ",trads="",pseuds="! ";
        for (int i=0;i<translations.Count;i++){
            if (i<translations.Count-1) trads+=$"\"{translations[i]}\",";
            else trads+=$"\"{translations[i]}\"";
        }
        for (int i=0;i<pseudo_translations.Count;i++){
            if (i<pseudo_translations.Count-1) trads+=$"\"{pseudo_translations[i]}\",";
            else trads+=$"\"{pseudo_translations[i]}\"";
        }
        return init+trads+pseuds+prob;
    }
    public void print(){
        Console.WriteLine(show());
    }

}