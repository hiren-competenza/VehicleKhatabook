"use client";
import React, { useState, useEffect } from 'react';
import { Form, FormGroup, Label, Input, Button, Row, Col, Table } from 'reactstrap';
import { Switch } from '@mui/material';
import { addVehicleType, getVehicleType, updateVehicleType ,getLanguageType} from '@/service/admin.service';

const Page = () => {
    const [vehicleTypeData, setVehicleTypeData] = useState({
        vehicleTypeId: 0,
        typeName: "",
        isActive: true,
        VehicleTypeLanguageJson: "",
    });
    const [vehicleTypes, setVehicleTypes] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState("");
    const [isEditMode, setIsEditMode] = useState(false);
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage] = useState(5); // Set the number of items per page
    const [languageData, setLanguageData] = useState([]);
    const [languageInputs, setLanguageInputs] = useState<Record<number, string>>({});
    const [successMessage, setSuccessMessage] = useState(""); // State for success message

    const handleChange = (e: any) => {
        const { name, value, type, checked } = e.target;
        setVehicleTypeData((prevData) => ({
            ...prevData,
            [name]: type === "checkbox" ? checked : value,
        }));
        
    };
    const handleGenerateJSON = () => {
        const jsonData = languageData.map((language: any) => ({
          languageTypeId: language.languageTypeId,
          languageName: language.languageName,
          translatedLanguage: languageInputs[language.languageTypeId] || "",
        }));
        return JSON.stringify(jsonData);
      };
      const fetchVehicleTypeData = async () => {
        const languages = await getLanguageType();
        setLanguageData(languages)
        setIsLoading(true);
        setError("");
        try {
            const data = await getVehicleType();  // Assume this fetches all data
            setVehicleTypes(data);
        } catch (error) {
            setError("Error fetching vehicle type data");
            console.error(error);
        } finally {
            setIsLoading(false);
        }
    };
    const handleSubmit = async (e: any) => {
        e.preventDefault();
    
        // Check if any language fields are empty
        const incompleteFields = languageData.some(
          (language: any) => !languageInputs[language.languageTypeId]?.trim()
        );
    
        if (incompleteFields) {
          const confirmFillDefault = window.confirm(
            `Some language fields are empty. If you proceed, the default language value will be the "${vehicleTypeData.typeName}". Do you want to continue?`
          );
    
          if (confirmFillDefault) {
            // Fill empty fields with the default value (vehicleTypeData.typeName)
            const updatedLanguageInputs = { ...languageInputs };
            languageData.forEach((language: any) => {
              if (!updatedLanguageInputs[language.languageTypeId]?.trim()) {
                updatedLanguageInputs[language.languageTypeId] = vehicleTypeData.typeName;
              }
            });
            setLanguageInputs(updatedLanguageInputs);
            return;

          } else {
            return;
          }
        }
    
        try {
          // Generate the JSON for language data
          const languageJson = handleGenerateJSON();
          const updatedData = {
            ...vehicleTypeData,
            VehicleTypeLanguageJson: languageJson, // Add the language JSON to the data
          };
    
          if (isEditMode) {
            // If editing, update the existing vehicle type
            await updateVehicleType(updatedData);
          } else {
            // If adding a new vehicle type
            await addVehicleType(updatedData);
          }
          setSuccessMessage(isEditMode ? "Vehicle Type Management updated successfully!" : "Vehicle Type Management added successfully!");

          // Reset form state after successful submission
          setIsEditMode(false);
          setVehicleTypeData({
            vehicleTypeId: 0,
            typeName: "",
            isActive: true,
            VehicleTypeLanguageJson: "",
          });
          setLanguageInputs({}); // Reset language inputs
          fetchVehicleTypeData(); // Fetch updated vehicle type data
          setTimeout(() => setSuccessMessage(""), 3000);

        } catch (error) {
          console.error("Error saving vehicle type data:", error);
        }
    };
    
      
    

 
    const handleEdit = (vehicleType: any) => {
        // Set the expense category data for editing
        setVehicleTypeData({
            vehicleTypeId: vehicleType.vehicleTypeId,
            typeName: vehicleType.typeName,
            isActive: vehicleType.isActive || "",
            VehicleTypeLanguageJson: vehicleType.VehicleTypeLanguageJson || "",
          
        });
      
        // Parse the JSON and update language inputs
        try {
          const parsedJson = JSON.parse(vehicleType.vehicleTypeLanguageJson
          );
          
          if (Array.isArray(parsedJson)) {
            // Map the parsed JSON to languageInputs
            const updatedLanguageInputs = parsedJson.reduce((acc: any, language: any) => {
              acc[language.languageTypeId] = language.translatedLanguage; // Assuming field name is 'translatedLanguage'
              return acc;
            }, {});           

            setLanguageInputs(updatedLanguageInputs);
          }
          setIsEditMode(true); // Set the edit mode to true when editing

        } catch (error) {
          console.error("Error parsing JSON description:", error);
          setLanguageInputs({});
        }
      };
    const handleCancelEdit = () => {
        setVehicleTypeData({
            vehicleTypeId: 0,
            typeName: "",
            isActive: true,
            VehicleTypeLanguageJson: "",
        });
        setLanguageInputs({});

        setIsEditMode(false);
    };

    const paginateData = (data: any[]) => {
        const startIndex = (currentPage - 1) * itemsPerPage;
        return data.slice(startIndex, startIndex + itemsPerPage);
    };
    const handleLanguageInputChange = (e: React.ChangeEvent<HTMLInputElement>, languageTypeId: number) => {
        const { value } = e.target;
        setLanguageInputs((prev) => ({
          ...prev,
          [languageTypeId]: value,
        }));
      };
    
    const handlePageChange = (page: number) => {
        setCurrentPage(page);
    };

    useEffect(() => {
        fetchVehicleTypeData();
    }, []);

    useEffect(() => {
        setCurrentPage(1);  // Reset to page 1 when data changes
    }, [vehicleTypes]);

    const totalPages = Math.ceil(vehicleTypes.length / itemsPerPage);

    return (
        <div className="mt-4 ml-4" style={{ textAlign: 'left', marginLeft: '10px' }}>
            <h3>Vehicle Type Management</h3>

            <Form onSubmit={handleSubmit} className="vehicle-type-form">
                <Row>
                    <Col sm={12} md={6}>
                        <FormGroup>
                            <Label for="typeName">Type Name</Label>
                            <Input
                                type="text"
                                name="typeName"
                                id="typeName"
                                placeholder="Enter type name"
                                value={vehicleTypeData.typeName}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col sm={12} md={6}>
                        <FormGroup>
                            <Label>Is Active</Label>
                            <Switch
                                checked={vehicleTypeData.isActive}
                                name="isActive"
                                color="primary"
                                onChange={(e) => setVehicleTypeData({ ...vehicleTypeData, isActive: e.target.checked })}
                                
                            />
                        </FormGroup>
                    </Col>
                </Row>
                <h3>Language Management</h3>
        <table className="table table-bordered">
          <thead>
            <tr>
              <th>Language Name</th>
              <th>Input</th>
            </tr>
          </thead>
          <tbody>
            {languageData.map((language: any) => (
              <tr key={language.languageTypeId}>
                <td>{language.languageName}</td>
                <td>
                  <Input
                    type="text"
                    placeholder="Enter value"
                    value={languageInputs[language.languageTypeId] || ""}
                    onChange={(e) => handleLanguageInputChange(e, language.languageTypeId)}
                  />
                </td>
              </tr>
            ))}
          </tbody>
        </table> 
        {successMessage && <div className="alert alert-success mt-3">{successMessage}</div>}

                <div className="button-group d-flex flex-column flex-sm-row">
                    <Button
                        color="primary"
                        type="submit"
                        className="submit-button"
                        style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
                    >
                            {isEditMode ? 'Update Vehicle Type' : 'Add Vehicle Type'}
                            </Button>

                    {isEditMode && (
                        <Button
                            color="secondary"
                            type="button"
                            onClick={handleCancelEdit}
                            className="ms-2 action-button"
                            style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
                        >
                            Cancel
                        </Button>
                    )}
                </div>

            </Form>

            <Row className="mt-4">
                <Col sm={12}>
                    <h4>Vehicle Type Data</h4>
                    {isLoading && <p>Loading...</p>}
                    {error && <p>{error}</p>}
                    {vehicleTypes.length === 0 ? (
                        <p>No vehicle types found.</p>
                    ) : (
                        <Table className="table table-bordered">
                            <thead>
                                <tr>
                                    <th style={{ backgroundColor: '#f8f9fa' }}>Type Name</th>
                                    <th style={{ backgroundColor: '#f8f9fa' }}>Status</th>
                                    <th style={{ backgroundColor: '#f8f9fa' }}>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {paginateData(vehicleTypes).map((vehicleType: any) => (
                                    <tr key={vehicleType.vehicleTypeId}>
                                        <td>{vehicleType.typeName}</td>
                                        <td>{vehicleType.isActive ? 'Active' : 'Inactive'}</td>
                                        <td>
                                            <Button
                                                size="sm"
                                                style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
                                                onClick={() => handleEdit(vehicleType)}
                                            >
                                                Edit
                                            </Button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </Table>
                    )}
                    
                    <div className="pagination-controls">
                        <Button
                            color="secondary"
                            onClick={() => handlePageChange(currentPage - 1)}
                            disabled={currentPage === 1}
                            style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}

                        >
                            Prev
                        </Button>
                        <span>Page {currentPage} of {totalPages}</span>
                        <Button
                            color="secondary"
                            onClick={() => handlePageChange(currentPage + 1)}
                            disabled={currentPage === totalPages}
                            style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}

                        >
                            Next
                        </Button>
                    </div>
                </Col>

            </Row>

            <style jsx>{`
                .form-container {
                    max-width: 100%;
                    margin: 0 auto;
                    padding: 15px;
                    background-color: #f8f9fa;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }
                .button-group {
                    display: flex;
                    flex-direction: column;
                    gap: 10px;
                    margin-top: 20px;
                }
                .vehicle-type-form {
                    margin-bottom: 20px;
                }
                table {
                    margin-top: 20px;
                    width: 100%;
                    border-collapse: collapse;
                }
                .table-responsive {
                    overflow-x: auto;
                }
                
                @media (max-width: 576px) {
                    .table-responsive {
                        -webkit-overflow-scrolling: touch;
                    }
                }
            `}</style>
        </div>
    );
};

export default Page;
