import { Subset } from "app/extraClasses/subsets";

export class DashboardResponse {
    success: number;
    errorMessage: string;
    data: Subset[];
  }