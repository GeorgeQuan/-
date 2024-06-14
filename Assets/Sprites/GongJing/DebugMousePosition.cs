using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class DebugMousePosition : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Input.mousePosition);
        }

    }
}

public class TreeNode
{
    public int val;
    public TreeNode left;
    public TreeNode right;
    public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
    {
        this.val = val;
        this.left = left;
        this.right = right;
    }
}

public class Solution
{
    public bool IsValidBST(TreeNode root)
    {

        if (root == null) return false;
        return DeGui(root);

    }
    public bool DeGui(TreeNode root)
    {
        if (root.left == null && root.right == null)
        {
            return true;
        }
        else if (root.left == null)
        {
            if (root.right.val > root.val)
            {
                return DeGui(root.right);
            }
            else
            {
                return false;
            }
        }
        else if (root.right == null)
        {
            if (root.left.val < root.val)
            {
                return DeGui(root.left);
            }
            else
            {
                return false;
            }
        }
        else if (root.left.val < root.val && root.right.val > root.val)
        {
            bool a = DeGui(root.left);
            if (a)
            {
                return DeGui(root.right);
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
     
    }
}
