using System;    
using System.Collections.Generic;    
using System.Linq;    
using System.Text;    
using System.Text.RegularExpressions;    
using System.Net;    
using System.Net.Sockets;    
using System.Net.Security;    
using System.IO;  

namespace Mail_Checker
{    
   
    
class ImapClient    
{    
     TcpClient _tcpClient;    
     StreamReader _reader;    
     StreamWriter _writer;
    SslStream streamSsl;
    NetworkStream stream;
    public string LastResponse
    {
        get
        {
            return lastResponse;
        }
    }
    string lastResponse;
    protected string _selectedFolder = string.Empty;    
    protected int _prefix = 1;  
  
   
    public bool ConnectImap(string host, int port, string proxyHost, int proxyPort, int timeout, bool ssl = true)    
    {

        xNet.Net.HttpProxyClient prox = new xNet.Net.HttpProxyClient(proxyHost, proxyPort);
        prox.ConnectTimeout = timeout;
        prox.ReadWriteTimeout = timeout;

            _tcpClient = prox.CreateConnection(host, port);  

   
            if (ssl)    
            {
                stream = _tcpClient.GetStream();
                streamSsl = new SslStream(stream);
                streamSsl.ReadTimeout = timeout;
                streamSsl.WriteTimeout = timeout;
                streamSsl.AuthenticateAsClient(host);    
                
   
                _reader = new StreamReader(streamSsl);    
                _writer = new StreamWriter(streamSsl);  
            }    
            else    
            {    
                var stream = _tcpClient.GetStream();    
                _reader = new StreamReader(stream);    
                _writer = new StreamWriter(stream);    
            }

            lastResponse = _reader.ReadLine();

            if (lastResponse.Contains("OK")) return true;
        return false;

    }


   
    public bool Authenicate(string user, string pass)    
    {    
        this.SendCommand(string.Format("LOGIN {0} {1}", user, pass));
        lastResponse = this.GetResponse();

        if (lastResponse.Contains("OK")) return true;
        return false;
    }    
   
    public List<string> GetFolders()    
    {    
        this.SendCommand("LIST \"\" *");
        lastResponse = this.GetResponse();

        string[] lines = lastResponse.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);    
        List<string> folders = new List<string>();

        for (int i = 0; i < lines.Length-1; i++)
        {
            string[] folderLine = lines[i].Split(' ');
            folders.Add(folderLine[folderLine.Length - 1]);
        }  
   
        return folders;    
    }    
   
    public bool SelectFolder(string folderName)    
    {    
        this._selectedFolder = folderName;    
        this.SendCommand("SELECT " + folderName);
        lastResponse = this.GetResponse();

        if (lastResponse.Contains("OK")) return true;
        return false;
    }

    public int SearchFrom(string query)
    {
        this.SendCommand("SEARCH FROM " + query);
        lastResponse = this.GetResponse();
        if (lastResponse.Contains("SEARCH\r\n")) return 0;
        if (lastResponse.Contains("SEARCH ")) return (lastResponse.Split(' ').Length - 5);
        return -1;
    }
   
    protected void SendCommand(string cmd)    
    {    
        _writer.WriteLine("A" + _prefix.ToString() + " " + cmd);    
        _writer.Flush();    
        _prefix++;    
    }    
   
    protected string GetResponse()    
    {    
        string response = string.Empty;    
   
        while (true)    
        {
            string line = _reader.ReadLine();
            if (line == null) throw new IOException();

            string[] tags = line.Split(new char[] { ' ' });    
            response += line + Environment.NewLine;    
            if (tags[0].Substring(0,1) == "A" && tags[1].Trim() == "OK" || tags[1].Trim() == "BAD" || tags[1].Trim() == "NO")    
            {    
                break;    
            }    
                
        }    
   
        return response;    
    }

    public void Dispose()
    {
        if (_reader != null) _reader.Dispose();
        if (_writer != null) _writer.Dispose();
        if (streamSsl!=null) streamSsl.Close();
        if (streamSsl != null) streamSsl.Dispose();
        if (stream != null) stream.Close();
        if (stream != null) stream.Dispose();
        if (_tcpClient != null) _tcpClient.Close();
    }
} 
}
