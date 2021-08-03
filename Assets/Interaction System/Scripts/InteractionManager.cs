using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] List<CharacterAgent> SelectedCharacters = new List<CharacterAgent>();
    [SerializeField] float MaxRaycastDistance = 100f;
    [SerializeField] LayerMask RaycastMask = ~0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && SelectedCharacters.Count > 0)
            ProcessClickCommand();
        if (Input.GetMouseButtonDown(0))
            ProcessSelectCommand();
    }

    void ProcessSelectCommand()
    {
        // convert mouse location to world space ray
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // perform raycast
        RaycastHit hitInfo;
        if (Physics.Raycast(cameraRay, out hitInfo, MaxRaycastDistance, RaycastMask, QueryTriggerInteraction.Ignore))
        {
            if (hitInfo.collider.CompareTag("Character"))
            {
                // retrieve the character script and ensure it is controllable by us
                var character = hitInfo.collider.GetComponentInParent<CharacterAgent>();
                if (character != null && character.Faction == EFaction.Player)
                {
                    // add or remove the character depending on if it is already selected
                    if (SelectedCharacters.Contains(character))
                        SelectedCharacters.Remove(character);
                    else
                        SelectedCharacters.Add(character);
                }
            }
        }
    }

    void ProcessClickCommand()
    {
        // convert mouse location to world space ray
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // perform raycast
        RaycastHit hitInfo;
        if (Physics.Raycast(cameraRay, out hitInfo, MaxRaycastDistance, RaycastMask, QueryTriggerInteraction.Ignore))
        {
            if (hitInfo.collider.CompareTag("Ground"))
                IssueCommand_Move(hitInfo.point);
        }
    }

    void IssueCommand_Move(Vector3 point)
    {
        foreach(var character in SelectedCharacters)
            character.MoveTo(point);
    }
}
