namespace TreinoWeb.Models
{
    public class TreinosModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int   ExerciciosId { get; set; }

        public virtual ExercicioModel Exercicios { get; set; }

    }
}
