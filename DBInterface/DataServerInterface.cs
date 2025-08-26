using System.Drawing;
using System.ServiceModel;
using DBLib;

namespace DBInterface
{
    [ServiceContract]
    public interface DataServerInterface
    {
        [OperationContract]
        int GetNumEntries();

        [OperationContract]
        [FaultContract(typeof(IndexOutOfRangeFault))]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon);

        [OperationContract]
        DataStruct SearchByLastName(string lastName);
    }
}
