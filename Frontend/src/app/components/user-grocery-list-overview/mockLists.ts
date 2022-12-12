import {GroceryList} from "../../interfaces/GroceryList";
import {Status} from "../../interfaces/StatusEnum";

export const MockLists: GroceryList[] =
  [
      {
        id: 1,
        title: "Test List 1 with a very very very veryveryvery very very long titleeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee",
        items: [
          {
            id: 1,
            title: "Test Item 1a very very very veryveryvery very very long titl",
            quantity: 1,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Fruits",
            index: 0
          },
          {
            id: 2,
            title: "Test Item 2",
            quantity: 2,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Fruits",
            index: 1
          },
          {
            id: 3,
            title: "Test Item 3",
            quantity: 3,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Fruits",
            index: 2
          },
          {
            id: 4,
            title: "Test Item 4",
            quantity: 4,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Fruits",
            index: 3
          },
          {
            id: 5,
            title: "Test Item 5",
            quantity: 5,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Fruits",
            index: 4
          },
          {
            id: 6,
            title: "Test Item 6",
            quantity: 6,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Fruits",
            index: 5
          },
          {
            id: 7,
            title: "Test Item 7",
            quantity: 7,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Fruits",
            index: 6
          },
          {
            id: 8,
            title: "Test Item 8",
            quantity: 8,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Fruits",
            index: 7
          },
          {
            id: 9,
            title: "Test Item 9",
            quantity: 9,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Test Category 1",
            index: 8
          },
          {
            id: 10,
            title: "Test Item 10",
            quantity: 10,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Test Category 1",
            index: 9
          },
          {
            id: 11,
            title: "Test Item 11",
            quantity: 11,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Test Category 1",
            index: 10
          },
          {
            id: 12,
            title: "Test Item 12",
            quantity: 12,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Test Category 1",
            index: 11
          },
          {
            id: 13,
            title: "Test Item 13",
            quantity: 13,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Test Category 1",
            index: 12
          },
          {
            id: 14,
            title: "Test Item 14",
            quantity: 14,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Test Category 1",
            index: 13
          },
          {
            id: 15,
            title: "Test Item 15",
            quantity: 15,
            groceryListId: 1,
            status: Status.Unchecked,
            category: "Test Category 1",
            index: 14
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
            status: Status.Unchecked,
            category: "Test Category 1",
            index: 0
          },
          {
            id: 4,
            title: "Test Item 4",
            quantity: 4,
            groceryListId: 2,
            status: Status.Unchecked,
            category: 'meat',
            index: 1
          }
        ]
      },
      {
        id: 3,
        title: "Test List 3",
        items: [
          {
            id: 5,
            title: "Test Item 5",
            quantity: 5,
            groceryListId: 3,
            status: Status.Unchecked,
            category: "Fruits",
            index: 0
},
          {
            id: 6,
            title: "Test Item 6",
            quantity: 6,
            groceryListId: 3,
            status: Status.Unchecked,
            category: "Fruits",
            index: 1
          }]
      },
      {
        id: 4,
        title: "Test List 4",
        items: [
          {
            id: 7,
            title: "Test Item 7",
            quantity: 7,
            groceryListId: 4,
            status: Status.Unchecked,
            category: "Fruits",
            index: 1
          }];


