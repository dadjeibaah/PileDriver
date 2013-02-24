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

( (*in*)  n, i, j : INTEGER ) ;

VAR
    k                  (* k will be set to the peg # i or j *)
        : INTEGER ;

BEGIN
    IF n = 1 THEN
		WRSTR ("Move ");
        WRINT ( i ) ;
		WRSTR (" to ");
        WRINT ( j ) ;
		WRSTR (".");
        WRLN ;
    ELSE
        k := 6 - i - j ;               (* compute number of third peg *)
        towersOfHanoi ( n-1, i, k ) ;  (* move n-1 disks from i to k  *)
        towersOfHanoi (   1, i, j ) ;  (* move   1 disk  from i to j  *)
        towersOfHanoi ( n-1, k, j ) ;  (* move n-1 disks from k to j  *)
    END ;
END towersOfHanoi ;

BEGIN  (* main *)
	WRSTR ("Enter a number of disks. (One who enters fewer than 10 shows great wisdom.) ");
    numDisks := RDINT () ;
	WRLN; WRLN;
    towersOfHanoi ( numDisks, 1, 3 ) ;
END towers .
