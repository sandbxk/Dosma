import {ListItem} from "./ListItem";

export interface GroceryList {
  id: number;
  title: string;
  listItems: ListItem[];
  created: Date;
  modified: Date;
}
