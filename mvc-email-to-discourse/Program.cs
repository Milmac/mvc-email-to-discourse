using System.Xml.Linq;

class Programs {
    public static void Main(String[] args) {

        string xmlFileLocation = args[0];
        Console.WriteLine("Using XML file from " + xmlFileLocation);

        //Load XML document from parameter
        XDocument xml = XDocument.Load(xmlFileLocation);


        
        //Run query to retrieve Usernames and Emails from xml file
        var query = from c in xml.Root.Descendants("MembershipUser")
                    select 
                    $"if user = User.find_by(username: '{c.Element("UserName").Value}')\n" +
                    $"\tuser.update(email: \"{c.Element("Email").Value}\")\n" +
                    //$"\tputs 'User {c.Element("UserName").Value} found! Updating email'\n" +
                    $"\tsuccessUsers += 1\n" +
                    $"else\n" +
                    $"\tputs 'User {c.Element("UserName").Value} is not found, womp womp'\n" +
                    $"\tfailedUsers += 1\n" +
                    $"end\n";

        //Where the command will be saved to
        string outputLocation = $"C:\\Users\\mille\\Downloads\\import-to-discourse-tool\\discourse-ruby-email-command-{DateTime.Now:HH-mm-ss}.rb";

        //Combine and output joined query result into ruby command
        using StreamWriter outputFile = new(outputLocation);

        outputFile.WriteLine("successUsers = 0");
        outputFile.WriteLine("failedUsers = 0\n");

        foreach (var command in query) {
            outputFile.WriteLine(command);
        }

        outputFile.WriteLine("puts 'There were ' + successUsers.to_s + ' user emails Successfully Updated and ' + failedUsers.to_s + ' emails that Failed to update.'");
        Console.WriteLine($"Wrote {query.Count()} email update commands to {outputLocation}");
    }
}