public class UI_Bag : UI_Base
{
    enum Items
    {
        First,
        Second,
        Third,
        Fourth,
        Fifth,
        Sixth
    }
    
    public override void Init()
    {
        Bind<UnityEngine.UI.Image>(typeof(Items));
    }
    
    public void BindBag(Bag bag)
    {
        bag.ContentChange += ShowItems;
    }

    private void ShowItems(Bag bag)
    {
        for (int i = 0; i < 6; i++)
        {
            var currentItem = bag.Items[i];
            var itemUI = GetImage((Items)i).transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
            if (currentItem != null)
            {
                // TODO - item Image 넣기
                //itemUI.image = currentItem.image;
                itemUI.enabled = true;
            }
            else
                itemUI.enabled = false;
        }
    }
}
