; This must be included in the data segment.
;
; The following are used for floating point (REAL) numbers:
;
r_f0      DD  ?          ; store EAX while using int 21h commands
r_f1      DD  12.25      ; float value = 1100.01 = 1.10001 * 2^3 
; sign: 0   exponent (127 bias -> 130) 10000010   10001000 ... 
; so 0100 0001 0100 0100 0000 0000 0000 0000
; or 41 44 00 00
;
; strings used in REAL procedures:
rstr2       DB  '   |   |   |   |   |   |   |   |',0
rstrThree0  DB  '000',0
rstrTwo0    DB  '00',0
rstrOne0    DB  '0',0

; Include the constants discovered in the program (rc0, rc1, etc.)
;INCLUDE rc.inc
; Include the real variables discovered in the program (r0, r1, r2, etc.)
;INCLUDE r.inc
; Include the real literals discovered in the program (rlit0, rlit1, etc.)
INCLUDE rlit.inc

