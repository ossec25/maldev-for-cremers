# Méthodologie

## Objectif
•	comprendre comment fonctionne un EDR
•	analyser la différence entre détection statique et détection comportementale
•	replacer la technique d’obfuscation utilisée dans l’exercice 3 dans un contexte réaliste de défense
 

## Mise en place
J'ai donc commencé par la mise en place d'une technique d'évasion basée sur l'obfuscation, l'idée étant de rendre le payload non identifiable par les mécanismes de détection statique de Defender basés sur des signatures (qui analysent un programme avant son exécution). Pour ce faire, j'ai modifié le shellcode avec un chiffrement basé sur une clé appliquée en XOR. Seule cette version chiffrée du payload apparait dans le binaire, lequel parait ne contenir qu'un simple tableau de données anodines. 
Le programme est cependant pourvu d'une phase de déchiffrement qui permet de reconstituer le payload original en mémoire juste avant son exécution. De cette manière, il n'est jamais écrit sur le disque de manière lisible. 

## Retour d'expérience
L'obfuscation seule a permis l'exécution du shellcode sur la machine test. Ceci indique que la structure et la lisibilité du code jouent un rôle important dans la détection et que celle-ci ne repose pas uniquement sur le comportement. J'ai cependant été étonné que l'utilisation conjuguée des API Win32 caractéristiques à la construction d'un malware n'ait même pas fait l'objet d'un message d'alerte.  

