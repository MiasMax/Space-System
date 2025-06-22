using UnityEngine;

public static class ObjectSelector
{
    public static Transform HandleClickSelection(Camera camera, Transform currentSelection)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.transform;
            }
            else
            {
                return null; // clic dans le vide -> désélection
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return null; // Échap -> désélection
        }

        return currentSelection; // pas de changement
    }

}