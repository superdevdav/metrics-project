public class MetricsRecord
{
      private int id;
      private string date_time;
      private long MMTB;
      private long PRMB;
      private double PCST;
      private long NRBT;
      private long NTBT;
      private long FSB;
      private long FFB;

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