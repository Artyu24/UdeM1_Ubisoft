using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public enum npcState
{
    Normal,
    Agressive,
    Fear
}
public enum npcType
{
    Passif,
    Actif
}
public class AIScript : MonoBehaviour
{
    private float anger;
    private float fear;
    [SerializeField] private LayerMask viewMak;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private NavMeshSurface navMesh;
    [SerializeField] Transform target;

   [field:SerializeField]public List<GameObject> interest { get; set; }
    void Start()
    {
        agent.destination = target.position;
    }

    void Update()
    {
        RaycastHit hit;
        Physics.Raycast( transform.position,(interest[0].transform.position - transform.position).normalized,out hit, 100, viewMak);
        Debug.DrawRay(transform.position, (interest[0].transform.position - transform.position).normalized * hit.distance, Color.yellow);

        if (hit.collider == interest[0].GetComponent<Collider>())//temp
        {
            if(Vector3.Distance(transform.position, interest[0].transform.position)<5&& agent.destination != interest[0].transform.position )
            {
                agent.destination= interest[0].transform.position;
            }
        }
    }
    
    /* un system de vision des element d'interet,
     tire un raycast si les points d'interet les plus proches, liste dynamique d'objet le points le plus proche a plus d'interet
     
     */



    /*le passif npc est constament aware du joueur mais n'y prete pas attention,
     si les action du joueur produise du bruit ou des mouvement d'objet le NPC gagne en peur
    si le npc a trop peur il fuis la zone

     npc actif ils cherchent les joueur,si il attrape un joueur il peuvent le relacher si il se fon attaquer, si les deux joueur son pick gameover, ils se deplace en
    cherchant les joeurs


    actif:
        cherche le joueurs se deplacent dans la zone.
                            si le joeur est proche ou dans leu champ de vision ils le suivent

     */

}
