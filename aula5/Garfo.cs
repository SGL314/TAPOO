namespace Fil{
    class Garfo{
        public int id;
        public bool use;
        public List<Filosofo> lista;
        
        public Garfo(int id){
            this.id = id;
            this.use = false;
            this.lista = new List<Filosofo>();
        }
    }
}