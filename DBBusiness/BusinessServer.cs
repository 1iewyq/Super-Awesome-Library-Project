using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Drawing;
using DBInterface;
using DBLib;

public class BusinessServer : BusinessServerInterface
{
    private uint LogNumber = 0;

    private DataServerInterface GetDataServer()
    {
        var tcp = new NetTcpBinding();
        var factory = new ChannelFactory<DataServerInterface>(tcp, "net.tcp://localhost:8100/DataService");
        return factory.CreateChannel();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private void Log(string logString)
    {
        LogNumber++;
        Console.WriteLine($"[Access Log #{LogNumber}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {logString}");
    }

    private bool IsValidLastName(string lastName)
    {
        return !string.IsNullOrWhiteSpace(lastName) &&
               Regex.IsMatch(lastName, @"^[a-zA-Z\-'\s]+$");
    }

    public int GetNumEntries()
    {
        Log("GetNumEntries called.");
        var dataServer = GetDataServer();
        int result = dataServer.GetNumEntries();
        Log($"GetNumEntries returned: {result}");
        return result;
    }

    public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon)
    {
        Log($"GetValuesForEntry called with index={index}.");
        var dataServer = GetDataServer();
        dataServer.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName, out icon);
        Log($"GetValuesForEntry returned: acctNo={acctNo}, pin={pin}, bal={bal}, fName={fName}, lName={lName}, icon={(icon != null ? "set" : "null")}");
    }

    public DataStruct SearchByLastName(string lastName)
    {
        Log($"SearchByLastName called with lastName='{lastName}'.");
        if (!IsValidLastName(lastName))
        {
            Log($"Invalid lastName input: '{lastName}'.");
            throw new ArgumentException("Last name contains invalid characters.");
        }

        try
        {
            var dataServer = GetDataServer();
            DataStruct result = dataServer.SearchByLastName(lastName);
            if (result == null)
            {
                Log($"No match found for lastName='{lastName}'.");
            }
            else
            {
                Log($"SearchByLastName returned: acctNo={result?.acctNo}, firstName={result?.firstName}, lastName={result?.lastName}");
            }
            return result;
        }
        catch (Exception ex)
        {
            Log($"Exception in SearchByLastName: {ex.Message}");
            throw;
        }
    }

    public async Task<DataStruct> SearchByLastNameAsync(string lastName)
    {
        Log($"SearchByLastNameAsync called with lastName='{lastName}'.");
        if (!IsValidLastName(lastName))
        {
            Log($"Invalid lastName input: '{lastName}'.");
            throw new ArgumentException("Last name contains invalid characters.");
        }

        try
        {
            var searchTask = Task.Run(() => SearchByLastName(lastName));
            var timeoutTask = Task.Delay(5000);

            var completedTask = await Task.WhenAny(searchTask, timeoutTask);
            if (completedTask == timeoutTask)
            {
                Log($"SearchByLastNameAsync timed out for lastName='{lastName}'.");
                throw new TimeoutException("Search operation timed out.");
            }

            DataStruct result = await searchTask;
            Log($"SearchByLastNameAsync completed for lastName='{lastName}'.");
            return result;
        }
        catch (Exception ex)
        {
            Log($"Exception in SearchByLastNameAsync: {ex.Message}");
            throw;
        }
    }
}
