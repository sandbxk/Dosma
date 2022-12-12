import {Status} from "./StatusEnum";

export interface Item {
  id: number;
  title: string;
  quantity: number;
  groceryListId: number;
  status: Status;
  category: string;
  index: number;
  

}
