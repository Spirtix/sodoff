namespace sodoff.Model {
    public class TaskStatus {
        public int Id { get; set; }

        public int MissionId { get; set; }

        public int VikingId { get; set; }

        public virtual Viking? Viking { get; set; }

        public string? Payload { get; set; }

        public bool Completed { get; set; } = false;
    }
}
