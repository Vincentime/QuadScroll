using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    private const float hauteurSection = 17.5f;
    public GameObject goSection;

    public List<GameObject> listSection;

    public int nbSection = 5;
    private int sectionActuel = 0;

    public Transform target;
    public float speedSection;
    public float speedPlayer;

    private bool moveToNextSection = false;
    private Vector3 targetPosition;
    private Rigidbody2D player;
    private Vector3 targetDebutSection;
    private float startTime;

    // Use this for initialization
    void Start()
    {
        //GameObject[] listSection = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject[] listSection = Resources.LoadAll<GameObject>("Section");

        player = GameObject.Find("PlayerTest").GetComponent<Rigidbody2D>();
        for (int i = 0; i < nbSection; i++)
        {
            int rand = Random.Range(0, listSection.Length);
            GameObject section = GameObject.Instantiate(listSection[rand], this.transform);
            section.name = "Section" + i;
            section.transform.position = this.transform.position;
            section.transform.position += Vector3.up * hauteurSection * i;
        }

    }


    // Update is called once per frame
    void Update()
    {

        if (moveToNextSection)
        {
            if (Vector3.Distance(player.position, targetDebutSection) > .25f)
            {
                 float step = speedPlayer * Time.deltaTime;
                player.transform.position = Vector3.MoveTowards(player.position, targetDebutSection, step);
            }

            if (Vector3.Distance(transform.position, targetPosition) > .25f)
            {
                float step = speedSection * Time.deltaTime;
                this.transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            }
            else
            {
                // Déplacement des sections fini
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                moveToNextSection = false;
                Debug.Log("fin déplacement section !");
                Physics2D.IgnoreLayerCollision(8, 9, false);
            }
        }


    }

    public void prochaineSection()
    {
        targetPosition = transform.position + Vector3.down * hauteurSection;
        string chemin = "Section" + sectionActuel + "/DebutSection";
        GameObject debutSectionSuivante = GameObject.Find(chemin);
        targetDebutSection = debutSectionSuivante.transform.position;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        Physics2D.IgnoreLayerCollision(9, 8, true);
        moveToNextSection = true;
        startTime = Time.time;
        sectionActuel++;
        Debug.Log("prochaine section");
    }
}
