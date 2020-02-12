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
        private Dictionary<TreeNode, ServerInfo> serverNodes;

        public Form1()
        {
            InitializeComponent();         
            InitializeTVServers();
        }

        private void InitializeTVServers()
        {
            SQLiteConnectionStringBuilder sBuilder = new SQLiteConnectionStringBuilder();

            //sBuilder.DataSource = @"E:\OneDrive\PSU\database\Repos\DB_UP_1_17.01.2020\Server_tree";
            sBuilder.DataSource = @"C:\Users\35498\OneDrive\PSU\database\Repos\DB_UP_1_17.01.2020\Server_tree";
            sBuilder.ForeignKeys = true; // не обязательно, т.к. не записываем данные

            serverNodes = new Dictionary<TreeNode, ServerInfo>();
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
                    long lastServerId = -1;
                    long lastGamerId = -1;
                    TreeNode currentServerNode = null;
                    TreeNode currentGamerNode = null;
                    while (reader.Read())
                    {
                        string serverName = reader["server_name"] as string;
                        long serverId = (long)reader["server_id"];
                        string gamerName = reader["gamer_name"] as string;
                        var gamerId = reader["gamer_id"];
                        string objectName = reader["object_name"] as string;

                        //Если идентификатор сервера поменялся, то создать новый узел дерева и сделать его текущим. Сохранить ссылку в словарь
                        if (lastServerId != serverId)
                        {
                            lastServerId = serverId;
                            currentServerNode = tvServers.Nodes.Add(serverName);
                            serverNodes.Add(currentServerNode, new ServerInfo(serverName));
                        }


                        if (gamerName != null) //Фактически значит: есть ли на сервере геймеры
                        {
                            //Если идентификатор геймера поменялся, то создаём новый узел дерева, принадлежащий серверу, и делаем его текущим узлом для добавления объектов.
                            if (lastGamerId != (long)gamerId) 
                            {
                                lastGamerId = (long)gamerId;
                                currentGamerNode = currentServerNode.Nodes.Add(gamerName);
                                serverNodes[currentServerNode].AddCountChl();

                                if (objectName != null) //Фактически значит: есть ли у геймера предметы
                                {
                                    //Если у нового геймера есть предмет, то он потомок Сервера с потомком. Увеличиваем число потомков текущего сервера, у которых есть потомки
                                    serverNodes[currentServerNode].AddCountChlWithChl();
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

            /// <summary>  
            ///  Возвращает число геймеров на сервере
            /// </summary> 
            public int CountChild { get { return countChild; } }
            /// <summary>  
            ///  Возвращает число геймеров на сервере, у которых есть предметы
            /// </summary> 
            public int CountChildWithChild { get { return countChildWithChild; } }
            public string Name { get { return name; } }

            public ServerInfo(string name)
            {
                this.name = name;
                this.countChild = 0;
                this.countChildWithChild = 0;
            }

            /// <summary>  
            ///  Увеличивает число потомков (геймеров) у сервера, у которых есть потомки (предметы)
            /// </summary> 
            public void AddCountChlWithChl()
            {
                countChildWithChild++;
            }

            /// <summary>  
            ///  Увеличивает число потомков у сервера
            /// </summary> 
            public void AddCountChl()
            {
                countChild++;
            }
        }


        private void tvServers_AfterExpand(object sender, TreeViewEventArgs e)
        {
            // Если раскрыли узел, являющийся сервером, изменяем его название в соответсвии с заданием
            if (serverNodes.ContainsKey(e.Node))
                e.Node.Text = $@"{e.Node.Text} - {serverNodes[e.Node].CountChildWithChild}/{serverNodes[e.Node].CountChild - serverNodes[e.Node].CountChildWithChild}";
        }


        private void tvServers_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            // Если закрыли узел, являющийся сервером, устанавливаем его название на название Сервера
            if (serverNodes.ContainsKey(e.Node))
                e.Node.Text = serverNodes[e.Node].Name;
        }
    }
}
