using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_UP_1_17_01_2020
{
    public partial class Form1 : Form
    {
        //List<KeyValuePair< server_node, KeyValuePair< count_Child, coun_Child_Without_Child >>>
        //private List<KeyValuePair<TreeNode, Tuple<int, int>>> servers;
        Dictionary<TreeNode, ServerInfo> servers;
        public Form1()
        {
            InitializeComponent();
            //servers = new List<KeyValuePair<TreeNode, Tuple<int, int>>>();
            servers = new Dictionary<TreeNode, ServerInfo>();
            InitializeTVServers();
        }

        private void InitializeTVServers()
        {
            SQLiteConnectionStringBuilder sBuilder = new SQLiteConnectionStringBuilder();
            sBuilder.DataSource = @"C:\Users\35498\source\repos\DB_UP_1_17_01_2020\game_database.sqlite";
            sBuilder.ForeignKeys = true;
            string sConnStr = sBuilder.ConnectionString;
            using (SQLiteConnection sConn = new SQLiteConnection(sConnStr))
            {

                sConn.Open();
                SQLiteCommand sCommand = new SQLiteCommand();
                sCommand.Connection = sConn;
                sCommand.CommandText = @"
                SELECT server.name as server_name,
                       server.id as server_id,
                       gamer.nickname as gamer_name,
                       gamer.id as gamer_id,
                       object.name as object_name
                FROM server
                    LEFT JOIN gamer ON server.id = gamer.server_id
                    LEFT JOIN object ON gamer.id = object.owner_gamer_id
                ORDER BY server_name,
                         server.id,
                         gamer_name,
                         gamer.id,
                         object_name,
                         object.id
                ";
                using (SQLiteDataReader reader = sCommand.ExecuteReader())
                {
                    long lastServer = -1;
                    long lastGamer = -1;
                    TreeNode currentServerNode = null;
                    TreeNode currentGamerNode = null;
                    while (reader.Read())
                    {
                        string serverName = reader["server_name"] as string;
                        long serverId = (long)reader["server_id"];
                        string gamerName = reader["gamer_name"] as string;
                        var gamerId = reader["gamer_id"];
                        string objectName = reader["object_name"] as string;

                        if (lastServer != serverId)
                        {
                            lastServer = serverId;
                            currentServerNode = tvServers.Nodes.Add(serverName);
                            servers.Add(currentServerNode, new ServerInfo(serverName));
                        }

                        if (gamerName != null)
                        {
                            if (lastGamer != (long)gamerId)
                            {
                                lastGamer = (long)gamerId;
                                currentGamerNode = currentServerNode.Nodes.Add(gamerName);
                                servers[currentServerNode].AddChl();
                                if (objectName != null)
                                {
                                    servers[currentServerNode].AddChlWithChl();
                                }
                            }
                            if (objectName != null)
                            {
                                currentGamerNode.Nodes.Add(objectName);
                            }
                        }
                    }
                }
            }
        }

        private class ServerInfo
        {
            private int countChild;
            private int countChildWithChild;

            private string name;

            public int CountChild { get { return countChild; } }
            public int CountChildWithChild { get { return countChildWithChild; } }
            public string Name { get { return name; } }

            public ServerInfo(string name)
            {
                this.name = name;
                this.countChild = 0;
                this.countChildWithChild = 0;
            }

            public void AddChlWithChl()
            {
                countChildWithChild++;
            }
            public void AddChl()
            {
                countChild++;
            }
        }

        private void tvServers_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void tvServers_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null && servers.ContainsKey(e.Node))
                e.Node.Text = $@"{e.Node.Text} - {servers[e.Node].CountChildWithChild}/{servers[e.Node].CountChild - servers[e.Node].CountChildWithChild}";


        }

        private void tvServers_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null && servers.ContainsKey(e.Node))
                e.Node.Text = servers[e.Node].Name;
        }
    }
}
