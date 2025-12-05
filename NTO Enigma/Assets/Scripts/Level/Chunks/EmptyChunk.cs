namespace NTO
{
    public sealed class EmptyChunk : Chunk
    {
        protected override void SubmarineEntered(Submarine submarine){}
        protected override void SubmarineLeave(Submarine submarine){}
        public override void SubmarineInside(Submarine submarine){}
    }
}