# Exercice 0 – Environnement de développement

## Mise en place
Création des deux machines virtuelles (de développement et d'observation) par clonage de machines Windows préconfigurées fournies dans l’environnement Ludus (Proxmox).

## VM développement
- Nom : ossecu46-maldev-dev-win11
- OS : Windows 11 Enterprise
- Rôle : développement et compilation des programmes
- Outils prévus :
  - Visual Studio Community 2022 (.NET desktop development)
  - Git
- Antivirus : désactivé
- Snapshot initial : ossecu46-maldev-dev-win11-1
  
## VM cible
- Nom : ossecu46-maldev-target-win11
- OS : Windows 11 Enterprise
- Rôle : exécution et observation des binaires
- Antivirus : Windows Defender actif
- Snapshot initial : ossecu46-maldev-target-win11-1

