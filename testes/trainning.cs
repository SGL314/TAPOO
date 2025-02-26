
class Trainning{
    public Trainning(){
        Main();
    }

    private void Main(){
        string f_words = "words.txt";
        if (File.Exists(f_words)){
            foreach (string line in File.ReadAllLines(f_words)) {
                bool collect = false,collectWord=true;
                foreach (char word in line) {
                    if (collect && collectWord){
                        
                    }
                    switch (word){
                        case """:
                            collect = (collect) ? false : true;
                            break;
                        case ":":

                    }
                }
            }
        }else{
            Console.WriteLine("Is not here.");
        }
    }
}