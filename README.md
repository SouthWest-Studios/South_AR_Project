
# 🕹️ BOOGLIGAN

**Una experiència de joc immersiva en realitat augmentada.**

## 📱 Descripció

**BOOGLIGAN** és una aplicació de realitat augmentada (AR) que ofereix una jugabilitat 360° on el jugador ha de girar-se físicament i utilitzar gestos tàctils per derrotar onades d’enemics. La combinació entre habilitat física i destresa tàctil crea una experiència dinàmica i única que fomenta la coordinació ull-mà-dispositiu.

## 🎯 Objectiu del Joc

Sobreviu a onades d’enemics que apareixen al teu voltant. Gira’t per trobar-los i derrota’ls fent gestos tàctils (lliscar el dit en diferents direccions). A mesura que avances, augmenta la dificultat i s’introdueixen nous desafiaments com bosses o enemics amb patrons diferents.

## 🌟 Punt d'Innovació

- **Immersió total**: L'usuari ha de moure’s físicament per trobar enemics i reaccionar-hi en temps real.
- **Coordinació avançada**: No n’hi ha prou amb tocar; cal interpretar la posició dels enemics i reaccionar correctament segons el seu comportament.
- **Sistema d’onades configurable**: Cada nivell és únic. Des de Unity es pot personalitzar la quantitat, velocitat, tipus i ritme d'aparició dels enemics.

## 🛠️ Desenvolupament

La idea inicial utilitzava gestos manuals com a sistema d’atac, però es va substituir per mecàniques tàctils (boles i gestos lliscants) degut a la complexitat del hand tracking. Aquest canvi va permetre una evolució més ràpida del joc i la implementació de múltiples modes:

### 🎮 Modes de Joc

- **Casual Mode**: 3 nivells — dos amb onades normals i un "Boss Level".
- **Infinite Mode**: Onades infinites, per veure fins on pots sobreviure.
- **Aim Mode**: Versió clàssica on els enemics són derrotats disparant boles amb tocs a la pantalla.

## 🧩 Característiques Clau

- Realitat augmentada 360°.
- Enemics amb comportaments diversos.
- Gestos tàctils per atacar (lliscar en direccions).
- Indicadors visuals per saber d’on venen els enemics.
- Editor d’onades personalitzable.
- Boss fight amb mecàniques complexes.

## 🧪 Problemes i Solucions

- ❌ **Hand Tracking** massa complicat → ✅ Interaccions tàctils (lliscar).
- ❌ Doble toc per atacar no funcionava bé → ✅ Es va substituir per lliscament d’un sol dit.
  
## 👥 Equip de Desenvolupament

| Membre | Aportació |
|--------|-----------|
| **Aleix** | `EnemySpawner`, `EnemyLogic`, lògica del mode infinit. |
| **Martí** | `BallSpawner`, `BossLevel`, `Frustrum Killer`. |
| **Lin** | `FingerMovement`, `EnemyWarnings`. |
| **Raül** | Disseny del joc, art i UI, models dels enemics. |

## 📌 Canvis Recents

Consulta l’historial de commits al [repo oficial](https://github.com/SouthWest-Studios/South_AR_Project/commits/1.0) per veure l’evolució del projecte: ajustos en la IA dels enemics, optimització del rendiment, i millores a la UI.

## 🧭 Com començar

1. Clona el repositori:
   ```bash
   git clone https://github.com/SouthWest-Studios/South_AR_Project.git
   ```
2. Obre el projecte a **Unity** (versió recomanada: `Unity 2021.3.X o superior`).
3. Assegura’t de tenir les dependències per AR Foundation i dispositius compatibles.
4. Exporta a dispositiu mòbil i gaudeix de la realitat augmentada!
