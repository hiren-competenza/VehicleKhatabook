// AdminAPI.ts

import { GET, POST, PUT, DELETE } from "./ApiMethods";
import { API_URL } from "./WebUrl";
export const getAdminData = async (body:any) => {
  
  return await POST(`${API_URL}/api/admin/Login`, body);
};
export const addLanguageType = async (body:any) => {
  
  return await POST(`${API_URL}/api/master/addLanguageType`, body);
};


export const addVehicleType = async (body:any) => {
  
  return await POST(`${API_URL}/api/master/addVehicleType`, body);
};


export const addIncomeCategory = async (body:any) => {
  
  return await POST(`${API_URL}/api/master/addIncomeCategory`, body);
};

export const addExpenseCategory = async (body: any) => {
  return await POST(`${API_URL}/api/master/addExpenseCategory`, body);
};


export const addSmsConfiguration = async (body:any) => {
  
  return await POST(`${API_URL}/api/master/addSMSProvider`, body);
};
export const updateLanguageType = async (body: any) => {
  const { languageTypeId } = body; 
  return await PUT(`${API_URL}/api/master/updateLanguageType/${languageTypeId}`, body);
};
export const updateVehicleType = async (body: any) => {
  const { vehicleTypeId } = body; 
  return await PUT(`${API_URL}/api/master/updateVehicleType/${vehicleTypeId}`, body);
};
export const updateSmsConfiguration = async (body: any) => {
  const { providerID } = body; 
  return await PUT(`${API_URL}/api/master/updateSMSProvider${providerID}`, body);
};
export const updateIncomeCategory = async (body: any) => {
  const { IncomeCategoryID } = body; 
  return await PUT(`${API_URL}/api/master/updateIncomeCategory/${IncomeCategoryID}`, body);
};
export const updateExpenseCategory = async (body: any) => {
  const { expenseCategoryID } = body; 
  return await PUT(`${API_URL}/api/master/updateExpenseCategory/${expenseCategoryID}`, body);
};
export const getVehicleType = async () => {
  try {
    const fullUrl = `${API_URL}/api/master/GetVehicleTypes`;

    const response = await GET(fullUrl);
    if (response && Array.isArray(response)) {
      return response; 
    } else if (response && response.data) {
      return response.data; 
        } else {
      return []; 
    }
  } catch (error) {
    return []; 
  }
};
export const getExpenseCategory = async () => {
  try {
    const fullUrl = `${API_URL}/api/master/GetExpenseCategories`;

    const response = await GET(fullUrl);
    if (response && Array.isArray(response)) {
      return response; 
    } else if (response && response.data) {
      return response.data; 
        } else {
      return []; 
    }
  } catch (error) {
    return []; 
  }
};

export const getIncomeCategory = async () => {
  try {
    const fullUrl = `${API_URL}/api/master/GetIncomeCategories`;
    const response = await GET(fullUrl);
    if (response && Array.isArray(response)) {
      return response; 
    } else if (response && response.data) {
      return response.data; 
        } else {
      return []; 
    }
  } catch (error) {
    return []; 
  }
};


export const getSmsConfiguration = async () => {
  try {
    const fullUrl = `${API_URL}/api/master/getAllSMSProvider`;

    const response = await GET(fullUrl);
    if (response && Array.isArray(response)) {
      return response; 
    } else if (response && response.data) {
      return response.data; 
        } else {
      return []; 
    }
  } catch (error) {
    return []; 
  }
};
export const getLanguageType = async () => {
  try {
    const fullUrl = `${API_URL}/api/master/GetAllLanguageTypes`;

    const response = await GET(fullUrl);
    if (response && Array.isArray(response)) {
      return response; 
    } else if (response && response.data) {
      return response.data; 
        } else {
      return []; 
    }
  } catch (error) {
    return []; 
  }  
};

