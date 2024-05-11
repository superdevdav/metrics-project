public class MetricsRecord
{
      public int id {get; private set;}
      public string date_time {get; private set;}
      public long MMTB {get; private set;}
      public long PRMB {get; private set;}
      public double PCST {get; private set;}
      public long NRBT {get; private set;}
      public long NTBT {get; private set;}
      public long FSB {get; private set;}
      public long FFB {get; private set;}

    public MetricsRecord(int id, string date_time, long MMTB, long PRMB, double PCST, long NRBT, long NTBT, long FSB, long FFB)
    {
        this.id = id;
        this.date_time = date_time;
        this.MMTB = MMTB;
        this.PRMB = PRMB;
        this.PCST = PCST;
        this.NRBT = NRBT;
        this.NTBT = NTBT;
        this.FSB = FSB;
        this.FFB = FFB;
    }
}