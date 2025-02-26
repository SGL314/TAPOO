class Word{
    string word;
    string[] traductions;
    string[] pseudo_traductions;
    float prob = 0;
    public Word(string word){
        this.word = word;
    }

    public void add(string add){
        traductions.Add(add);
    }
    public void remove(string remove){
        traductions.Remove(remove);
    }
    public string show(){
        string init = $"\"{word}\": ",trads="",pseuds="! ",;
        for (int i=0;i<traductions.Length;i++){
            if (traductions[i]<traductions.Length-1) trads+=$"\"{traductions[i]}\","
            else trads+=$"\"{traductions[i]}\"";
        }
        for (int i=0;i<pseudo_traductions.Length;i++){
            if (pseudo_traductions[i]<pseudo_traductions.Length-1) trads+=$"\"{pseudo_traductions[i]}\","
            else trads+=$"\"{pseudo_traductions[i]}\"";
        }
        return init+trads+pseuds+prob;
    }
    public void print(){
        Console.WriteLine(show());
    }

}