using System;
using System.Collections;
using System.Collections.Generic;		// ArrayList, HashTable, Stack
using System.Linq;

public class ATTRIBUTE
{
    protected ATTR_TYPE at_type;
    protected string ID_name;
    protected int iMemory;
    protected int iScope;
    protected object value;
    protected VAR_TYPE TypeVar;
    protected int AS, AE;
    protected bool isConst;

    public ATTRIBUTE()
    {
    }
    public ATTRIBUTE(string sName, int scope)
    {
        ID_name = sName;
        iScope = scope;
    }
    
    public ATTRIBUTE(string sName, int scope, ATTRIBUTE attr)
    {
        if (attr.at_type == ATTR_TYPE.TYPE)
        {
            at_type = ATTR_TYPE.VAR;
            int[] t = (int[])attr.value;
            iMemory = 4 * t.Length;
            value = attr.value;
            AS = attr.AS;
            AE = attr.AE;
        }
    }
    public ATTRIBUTE(string sName, int scope, object incvalue, bool Const)
    {
        ID_name = sName;
        iScope = scope;


        if (incvalue is int)
        {
            iMemory = 4;
            TypeVar = VAR_TYPE.INT;
            value = incvalue;
            if (Const)
            {
                at_type = ATTR_TYPE.CONST;
                isConst = true;
            }
            else isConst = false;
        }
        else if (incvalue.GetType().IsArray)
        {
            TypeVar = VAR_TYPE.TYPE;
            int[] array = (int[])incvalue;
            value = array;
            AS = array[0];
            AE = array[array.Length - 1];
        }
           
    }

    public int Scope
    {
        get { return iScope; }
    }
    public int Memory
    {
        get { return iMemory; }
    }

    public override string ToString()
    {
        return ID_name;
    }
    public void ChangeValue(int i)
    {
        if(!(at_type == ATTR_TYPE.CONST) && TypeVar == VAR_TYPE.INT)
        value = i;
    }

    public string GetTypeAsStr()
    {
       return at_type.ToString();
    }
    
}

public class PROC : ATTRIBUTE
{

    public PROC(string sName, int scope)
    {
        at_type = ATTR_TYPE.PROC;
        ID_name = sName;
        iScope = scope;
        iMemory = 0;
    }
}

public enum ATTR_TYPE
{
    VAR,
    TYPE,
    CONST,
    PROC,
    ARRAY
};
public enum VAR_TYPE
{
    TYPE,
    ARRAY,
    INT
};

/// <summary>
/// ATTRIBUTE constitutes each entry in the symbol table. It stores information needed by
///    the parser and emitter for each identifier (ID). ID's may be variables, constants,
///    or procedures.
/// </summary>

 // ATTRIBUTE class

/// <summary>
/// Procedure with Variable List. This handles one procedure completely. It stores the list
///    of variables (ArrayList of string), the local memory needed, 
///    and the parameter count for this one procedure.
/// </summary>
public class ProcVarList 
{
} // ProcVarList class


/// <summary>
/// The Symbol Table is a Singleton class that manages identifiers. It associates symbols 
///    with their attributes (ATTRIIBUTE, defined above).
///    
/// The symbol table design and layout, including the structures
///    stored within it were designed by John Hooper, The original
///    unix implementation was his also. It was modified extensively
///    by Tom Fuller in S 2000 and S 2002 for VC++ and modfied extensively 
///    again by Tom Fuller is W 2007 for C#.
///    
/// May 2002. Change the treatment of scope 0 by treating it as a procedure named (THF_main).
///    This required adding the handling of arrays and several changes in parse.cpp.
/// </summary>
public class SymbolTable
{
    public static int STACK_START = 4; // first byte of stack available for variables, explained next:
    private static SymbolTable smb_table;
    private static Object smb_table_lock = typeof(SymbolTable);
    private Hashtable table;
    private Stack ScopeStack;
    private List<int> TotalScope;
    private int iScopeNum;


    static public SymbolTable Instance()
    {
        lock (smb_table_lock)
        {
            if (smb_table == null)
            {
                smb_table = new SymbolTable();
                return smb_table;
            }

            return smb_table;
        }
    }

    private SymbolTable()
    {
        ScopeStack = new Stack();
        iScopeNum = -1;
        
        table = new Hashtable();
        TotalScope = new List<int>();
        

        
    }

    public void AddMaintoTable()
    {
        IncrementScope();
         PROC pMain = new PROC("Main",GetCurrScope());
        table.Add(String.Format("PD{0}_{1}",pMain.Scope,pMain.ToString()),pMain);  

    }
    public int MemoryForScope(int scopenum)
    {
        ArrayList al = new ArrayList(table.Values);
        int TotalStackSize = 0;
        var query = from ATTRIBUTE at in al
                    where at.Scope == scopenum
                    select at;

        foreach (ATTRIBUTE resultat in query)
        {
            TotalStackSize += resultat.Memory;
        }
                    
        return TotalStackSize;
    }
    public void IncrementScope()
    {
        iScopeNum++;
        ScopeStack.Push(iScopeNum);
        if (!TotalScope.Contains(iScopeNum))
        {
            TotalScope.Add(iScopeNum);
        }
        
    }
    public void DecrementScope()
    {
        if(iScopeNum >0)
        {
        
        ScopeStack.Pop();
        }
    }
    public int GetCurrScope()
    {
        return (int)ScopeStack.Peek();
    }
    public void AddSymbol(ATTRIBUTE attr)
    {
        
        table.Add(String.Format("PD{0}_{1}", attr.Scope, attr.ToString()), attr);
    }

    public string DumpSymTable()
    {
        string rtn = "SymbolTable Dump\r\n\r\n";
        AddMaintoTable();
        AddSymbol(new PROC("Fill",GetCurrScope()));
        IncrementScope();
        AddSymbol(new ATTRIBUTE("index",GetCurrScope()));
        AddSymbol(new ATTRIBUTE("TEST",GetCurrScope(),4,true));
        DecrementScope();
        AddSymbol(new PROC("Sort", GetCurrScope()));
        IncrementScope();
        AddSymbol(new ATTRIBUTE("aiArray",GetCurrScope(),new int[]{1,2,3,4},false));
        DecrementScope();

        rtn += "Memory for each scope\r\n";
        foreach (int i in TotalScope)
        {
            
            rtn += String.Format("Scope: {0,5}  Memory: {1,10}\r\n", i, MemoryForScope(i));
        }

        rtn+="**-----------------------------------------------------------------------------------------------------**\r\n";
        rtn += String.Format("{0}{1,16}{2,16}{3,16}{4,16}{5,16}{6,16}\r\n", "Attribute", "Type", "Memory", "Scope", "Value", "ArrayStart", "ArrayEnd");
//rtn+= "Attribute Nam\t\tAttribute Type\t\tMemory Needed\t\tScope\t\tValue\t\tArrayStart\t\tArrayEnd\r\n";
        rtn+="**-----------------------------------------------------------------------------------------------------**\r\n";

        foreach (DictionaryEntry item in table)
        {
            ATTRIBUTE newitem = (ATTRIBUTE)item.Value;
            rtn += String.Format("{0}{1,16}{2,16}{3,16}{4,16}{5,16}{6,16}\r\n", 
                item.Key.ToString(), newitem.GetTypeAsStr(), newitem.Memory, newitem.Scope,"test", "test","test");     
        }
        return rtn;
    }
    //I
    /// <summary>
    /// There are three functions that find names in the symbol table:
    /// FindSymbol -- Search for name; return a reference to the
    ///    ATTRIBUTE entry corresponding to the name ( if there is one), and
    ///    a bool to indicate whether the name was present
    ///    PRE:  symb_table is created;
    ///    POST: determines if ATTRIBUTE corresponding to name is present
    ///       and sets present to true if so, and false otherwise.
    ///       Return reference to the correct ATTRIBUTE, if present.
    ///       
    /// FindSymbol searches only within the current scope.
    /// FindInScope searches down the scope stack. 
    /// FindScopized searches for a "scopized" string. ("scopizing" is defined below).
    
    /*///<summary>
    void FindSymbol (string strName, out ATTRIBUTE Attr, out bool bPresent) 
    {   
    } // FindSymbol

    public void FindInScope (string strName, out ATTRIBUTE Attr, out bool bPresent) 
    {
    } // FindInScope

    /// <summary>
    /// Same as above, but with already-formed ("scopized") string.
    /// </summary>
    public void FindScopized (string strLookup, out ATTRIBUTE Attr, out bool bPresent) 
    {
    } // FindScopized
   */
} // SymbolTable class
