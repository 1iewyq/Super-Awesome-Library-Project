using System.Collections.Generic;
using System.ServiceModel;
using DBInterface;
using System.Drawing;
using DBLib;
using System.Threading.Tasks;


[ServiceContract]
public interface BusinessServerInterface
{
    [OperationContract]
    int GetNumEntries();

    [OperationContract]
    [FaultContract(typeof(IndexOutOfRangeFault))]
    void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon);

    /*[OperationContract]
    DataStruct SearchByLastName(string lastName);*/

    [OperationContract]
    Task<DataStruct> SearchByLastNameAsync(string lastName);
}
