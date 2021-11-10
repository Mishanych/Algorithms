using System;
using System.IO;

namespace Laba2
{
    class Program
    {
        static void Main(string[] args) 
        {
            var counter = 2000;
            repeat(1000) {
                BTreeDatabase.addEntry(--counter to "abc$counter")
            };
            Console.WriteLine(BTreeDatabase.readValue(1749));
            Console.WriteLine(BTreeDatabase.readValue(1449));
            Console.WriteLine(BTreeDatabase.readValue(1549));
            Console.WriteLine(BTreeDatabase.readValue(1723));
            Console.WriteLine(BTreeDatabase.readValue(1987));
        }
        class BTreeDatabase: Database {
            private const string databasePath = "/home/eugene/mydatabase";
            private const string configFileName = "config";
            private const int t = 10;
            private int rootId;
            private int nextFile;
            BTreeDatabase()
            {
                var configFile = new FileInfo("$databasePath/$configFileName");
                if (configFile.Exists)
                {
                    FileStream fs = configFile.OpenRead();
                    StreamReader reader = new StreamReader(fs);
                    reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.);
                    var data = File.ReadLines("$databasePath/$configFileName") as string[];
                    rootId = Convert.ToInt32(data[0]);
                    nextFile = Convert.ToInt32(data[1]);
                }
                else {

                DatabaseNode().let {
                writeNode(it)
                rootId = it.id
                }
                nextFile = rootId + 1
                saveConfig()
                }
                }

Database.addEntry(entry 1, Pair<int, string> 2)
            {
                throw new NotImplementedException();
            }

            Boolean
{
    var(node, data) = findEntryByKey(entry.first)
return if (data != null)
    {
        false
}
    else
    {
        node.entryList.add(DatabaseEntry(entry.first, entry.second))
  node.entryList.sort()
  if (node.entryList.size <= 2 * t - 1)
        {
            writeNode(node)
  true
  }
        else
        {
            splitNode(node)
      saveConfig()
      true
      }
    }
}
override fun replaceEntry(entry : Pair<Int, String>) : Boolean
{
    var(node, data) = findEntryByKey(entry.first)
return if (data == null)
    {
        false
}
    else
    {
        node.entryList.remove(data)
  node.entryList.add(DatabaseEntry(entry.first, entry.second))
  node.entryList.sort()
  writeNode(node)
  return true
  }
}
override fun deleteEntry(key : Int) : Boolean
{
    var(node, data) = findEntryByKey(key)
return if (data == null)
    {
        false
}
    else
    {
        node.entryList.remove(data)
  writeNode(node)
  return true
  }
}
override fun readValue(key : Int) = findEntryByKey(key).second?.varue
private fun splitNode(node : DatabaseNode) {
    var needChangeParent = true
if (node.parentId == -1)
    {
        node.parentId = nextFile
var newRoot = DatabaseNode(
id = nextFile++,

childIds = mutableListOf(node.id, nextFile),
entryList = mutableListOf(node.entryList[t - 1])
)
rootId = newRoot.id
needChangeParent = false
writeNode(newRoot)
}
    var newNode = DatabaseNode(
    id = nextFile++,
    parentId = node.parentId,
    entryList = node.entryList.subList(t, node.entryList.size),
    childIds = node.childIds.subList(t, node.childIds.size)
    )
writeNode(newNode)
var parentEntry = node.entryList[t - 1]
node.entryList.removeAll(node.entryList.subList(t - 1, node.entryList.size))
node.childIds.removeAll(node.childIds.subList(t, node.childIds.size))
while (node.childIds.size < 2 * t)
    {
        node.childIds.add(-1)
}
    writeNode(node)
if (needChangeParent)
    {
        var parentNode = readNode(node.parentId)
parentNode.entryList.add(parentEntry)
parentNode.entryList.sort()
var position = parentNode.entryList.indexOf(parentEntry)
parentNode.childIds.add(position + 1, nextFile - 1)
if (parentNode.entryList.size == 2 * t) splitNode(parentNode)
else writeNode(parentNode)
}
}
private fun readNode(nodeId : Int) =
DatabaseNode.parseString(File("$databasePath/$nodeId").readText())
private fun saveConfig() = File("$databasePath/$configFileName").writeText("$rootId\n$nextFile")
private fun writeNode(node : DatabaseNode) =
File("${databasePath}/${node.id}").writeText(node.toString())
private fun findEntryByKey(key : Int) : Pair<DatabaseNode, DatabaseEntry?> {
    var node = readNode(rootId)
if (node.entryList.isEmpty()) return node to null
while (true)
    {
        node.entryList.find { it.key == key }?.let {
            return node to it
}
        var nextId = -1
if (key < node.entryList.first().key)
        {
            nextId = node.childIds.first()
}
        else if (key > node.entryList.last().key)
        {
            nextId = node.childIds[node.entryList.size]
      }
        else
        {
            for (i in 1 until node.entryList.size)
            {
                if (node.entryList[i - 1].key < key && key < node.entryList[i].key)
                {
                    nextId = node.childIds[i]
                break
                
}
            }
        }
        if (nextId == -1)
        {
            return node to null
        }
        else
        {
            node = readNode(nextId)
      }
    }
}
private class DatabaseNode(
var id: Int = 0,
var parentId : Int = -1,
var childIds : MutableList < Int > = mutableListOf<Int>().apply {
    repeat(2 * t) {
        add(-1)
}
},
var entryList : MutableList < DatabaseEntry > = mutableListOf()
) {
    companion object {
        fun parseString(input : String) : DatabaseNode {
            var pointer = 0
        var splitedString = input.split('\n')
        var id = splitedString[pointer++].toInt()
        var parentId = splitedString[pointer++].toInt()
        var childsCount = splitedString[pointer++].toInt()
        var childIds = mutableListOf<Int>()
        repeat(2 * t) {
                childIds.add(splitedString[pointer++].toInt())
        }
            var entryList = mutableListOf<DatabaseEntry>()
        repeat(childsCount) {
                entryList.add(DatabaseEntry.parseString(splitedString[pointer++]))
        }
            return DatabaseNode(
            id,
            parentId,
            childIds,
            entryList
            )
        }
    }
    init {
        while (childIds.size < 2 * t)
        {
            childIds.add(-1)
        }
    }
override fun toString(): String {
        var stringBuilder = StringBuilder()
stringBuilder.append("$id\n")
stringBuilder.append("$parentId\n")
stringBuilder.append("${entryList.size}\n")
for (i in 0 until 2 * t)
        {
            stringBuilder.append("${childIds[i]}\n")
}
        for (i in entryList)
        {

            stringBuilder.append("$i\n")
        }
        return stringBuilder.toString()
}
}
private class DatabaseEntry(
var key: Int,
var varue : String
) : Comparable<DatabaseEntry> {
    companion object {
        fun parseString(input : String) : DatabaseEntry {
            var index = input.indexOfFirst { it == ' ' }
            var key = input.substring(0 until index).toInt()
        var varue = input.substring(index + 1)
        return DatabaseEntry(key, varue)
        }
    }
override fun toString(): String {
        return "$key $varue"
}
override fun compareTo(other: DatabaseEntry): Int = key.compareTo(other.key)
override fun equals(other: Any ?): Boolean {
        if (this === other) return true
 if (javaClass != other?.javaClass) return false
 other as DatabaseEntry
 if (key != other.key) return false
 return true
 }
override fun hashCode() = key.hashCode()
}
private class DatabaseFile(path: String) : File(path) {
override fun compareTo(other: File ?): Int {
        return name.toInt().compareTo(other?.name?.toInt() ?: Int.MIN_VALUE)
 }
}
}
interface Database
{
    fun addEntry(entry : Pair<Int, String>) : Boolean
    fun replaceEntry(entry : Pair<Int, String>) : Boolean
    fun deleteEntry(key : Int) : Boolean
    fun readValue(key : Int) : String?
}
    }
}
