MODULE towers ;

VAR
    numDisks          (* number of disks for current solution *)
        : INTEGER ;

(********************************************************************)
PROCEDURE towersOfHanoi
(* recursively solves the Towers of Hanoi problem;
   Pre:  n is the number of disks;
         i is the start peg;
         j is the goal peg;
   Post: if n = 1 output a move; otherwise decompose the problem into three
         simpler subproblems and solve in sequence:
         1.  move n-1 disks from i to k;
         2.  move 1   disk  from i to j ;
         3.  move n-1 disks from k to j ;
*)

( (*in*)  one, two, three : INTEGER ) ;

VAR
    k                  (* k will be set to the peg # i or j *)
        : INTEGER ;

BEGIN
        WRINT ( one ) ;
        WRSTR ( "  " ) ;
        WRINT ( two ) ;
        WRSTR ( "  " ) ;
        WRINT ( three ) ;
        WRLN ;
END towersOfHanoi ;

BEGIN  (* main *)
    WRSTR ( "Enter the number of disks (< 10 please) and press ENTER:  " ) ;
    WRLN ;
    numDisks := RDINT ();
    WRSTR ( "This should print the number entered, then 1, then 3." ) ;
    WRLN ;
    towersOfHanoi ( numDisks, 1, 3 ) ;
END towers.