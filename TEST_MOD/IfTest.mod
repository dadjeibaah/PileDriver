MODULE if_test ;

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
    IF one > three THEN
        WRINT ( one ) ;
        WRSTR ( " greater than " ) ;
        WRINT ( three ) ;
        WRLN ;
    ELSE
        WRINT ( three ) ;
        WRSTR ( " greater than or equal to " ) ;
        WRINT ( one ) ;
        WRLN ;
    END;

    IF one = one THEN
        WRINT ( one ) ;
        WRSTR ( " equals " ) ;
        WRINT ( one ) ;
        WRLN ;
    ELSE
        WRINT ( one ) ;
        WRSTR ( " is not equal to " ) ;
        WRINT ( one ) ;
        WRLN ;
    END;
END towersOfHanoi ;

BEGIN  (* main *)
    WRSTR ( "Enter the number of disks (< 10 please) and press ENTER:  " ) ;
    WRLN ;
    numDisks := RDINT () ;
    towersOfHanoi ( numDisks, 1, 3 ) ;
END if_test.