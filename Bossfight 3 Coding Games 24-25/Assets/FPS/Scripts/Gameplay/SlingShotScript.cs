using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public Transform startPoint; 
    public GameObject projectilePrefab; 
    private GameObject currentProjectile;
    private Vector2 initialMousePos;
    private bool isDragging = false;

    public float maxStretch = 3.0f;
    public float launchPower = 10f;

    void Update()
    {
        if (isDragging && currentProjectile != null)
        {
            DragProjectile();
        }
    }

    void OnMouseDown()
    {
        CreateProjectile();
        isDragging = true;
        initialMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        if (isDragging && currentProjectile != null)
        {
            LaunchProjectile();
            isDragging = false;
        }
    }
    
    void CreateProjectile()
    {
        if (currentProjectile == null)
        {
            currentProjectile = Instantiate(projectilePrefab, startPoint.position, Quaternion.identity);
            currentProjectile.GetComponent<Rigidbody2D>().isKinematic = true; 
        }
    }

    void DragProjectile()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - initialMousePos).normalized;

        float distance = Vector2.Distance(initialMousePos, mousePos);
        distance = Mathf.Min(distance, maxStretch);

        Vector2 newProjectilePos = initialMousePos + direction * distance;
        currentProjectile.transform.position = newProjectilePos;
    }

    void LaunchProjectile()
    {
        Vector2 launchDirection = (initialMousePos - (Vector2)currentProjectile.transform.position).normalized;
        float distance = Vector2.Distance(initialMousePos, currentProjectile.transform.position);
        
        Rigidbody2D rb = currentProjectile.GetComponent<Rigidbody2D>();
        rb.isKinematic = false; // Allow physics
        rb.velocity = launchDirection * distance * launchPower;
        
        currentProjectile = null;
    }
}
