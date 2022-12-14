import {Status} from "./StatusEnum";
import {CategoryEnum} from "./CategoryEnum";

export interface Item {
  id: number;
  title: string;
  quantity: number;
  groceryListId: number;
  status: Status;
  category: string;
  index: number;


}


export class ItemDTO {
  id: number = 0;
  title: string = "";
  quantity: number = 1;
  groceryListId: number = 0;
  status: Status = Status.Unchecked;
  category: string = 'None';

  toItem(): Item {
    return {
      id: this.id,
      title: this.title,
      quantity: this.quantity,
      groceryListId: this.groceryListId,
      status: this.status,
      category: this.category,
      index: 0
    }
  }

}
