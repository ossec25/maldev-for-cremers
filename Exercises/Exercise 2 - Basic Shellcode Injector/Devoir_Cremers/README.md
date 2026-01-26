# Méthodologie

## Observation/Analyse
A la lecture de l'énoncé et du programme figurant en solution d'exemple (toujours basé sur le shellcode msfvenom calc.exe), je constate qu'il s'agit ici d'injecter du code dans un autre processus que celui que j'utilise pour exécuter le code qui transporte le shellcode. Autrement dit, le shellcode devra s'exécuter dans un autre processus, ciblé par le programme qui le porte. 
Cela implique de : 
- Identifier la cible en tant que processus en cours (Appel à l'API Win32 **"OpenProcess"** pour obtenir le PID du processus cible en cours), lequel doit avoir une architecture x64 compatible  
- Obtenir un accès sur le processus cible avec des droits suffisants (un **handle**)
- Ecrire dans la mémoire de ce processus cible (Appel aux API Win32 **VitualAllocEx** et **WriteProcessMemory**)
- Déclencher l'exécution du shellcode dans ce processus cible (Appel à l'API Win32 **CreateRemoteThread**)

## Mise en place/retour d'expérience
J'ai repris le shellcode de démonstration **"MessageBox"** de msfvenom que j'avais déjà utilisé dans l'exercice 1 en adaptant son texte de diffusion à l'exercice. 
Ayant effectué des recherches quant aux processus cibles idéaux pour ce genre d'exercice, il semblerait que notepad (utilisé dans la solution en référence de l'exercice) soit particulièrement adapté d'un point de vue pédagogique (léger, stable, sans protections particulières, sans conséquences en cas de plantage). Il serait cependant très observé par les EDR et donc une véritable attaque aurait davantage de chance de porter ses fruits vers un IDE ou des logiciels métiers qui ont pour caractéristiques une mémoire très active et donc un bruit normal élevé (rendant une analyse plus complexe) mais qui ne sont pas pour autant davantage protégés ou surveillés. A cet égard, les processus Windows sont à proscrire, ceux-ci étant largement surveillés.  
Le processus cible notepad n'ayant jamais fonctionné, mes recherches sur internet m'ont indiqué que la version notepad figurant sur Windows 11 avait été modifiée et que cela pouvait être l'origine du problème. 
J'ai donc réalisé un petit programme Win32 x64 minimaliste en C# nommé **"TargetDummy"**, développé spécifiquement comme processus cible de test, sans logique de sécurité, fournissant un environnement contrôlé, stable et reproductible pour l'exécution de code dans un processus distant. Je joins ce petit programme dans les sources. 
Ceci m'a permis d'observer l'exécution du shellcode injecté (cfr screenshots en dossier src) sans être empêché par un notepad packagé sur Windows 11.  

 



