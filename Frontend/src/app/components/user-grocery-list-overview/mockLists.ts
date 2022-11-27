import {GroceryList} from "../../interfaces/GroceryList";

export const MockLists: GroceryList[] =
  [
      {
        id: 1,
        title: "Test List 1",
        items: [
          {
            id: 1,
            title: "Test Item 1",
            quantity: 1,
            groceryListId: 1,
            status: 1
          },
          {
            id: 2,
            title: "Test Item 2",
            quantity: 2,
            groceryListId: 1,
            status: 1
          }
        ]
      },
      {
        id: 2,
        title: "Test List 2",
        items: [
          {
            id: 3,
            title: "Test Item 3",
            quantity: 3,
            groceryListId: 2,
            status: 1
          },
          {
            id: 4,
            title: "Test Item 4",
            quantity: 4,
            groceryListId: 2,
            status: 1
          }
        ]
      }
    ];


