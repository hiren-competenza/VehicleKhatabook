"use client";
import React, { useState, useEffect } from 'react';
import { Form, FormGroup, Label, Input, Button, Row, Col } from 'reactstrap';
import { addExpenseCategory, getExpenseCategory, updateExpenseCategory, getLanguageType } from '@/service/admin.service';
import { Switch } from '@mui/material';

const Page = () => {
  const [expenseCategoryData, setExpenseCategoryData] = useState({
    expenseCategoryID: 0,
    name: "",
    RoleId: 1,
    description: "",
    isActive: true,
    expenseCategoryLanguageJson: "",
  });
  const [isEditMode, setIsEditMode] = useState(false);
  const [expenseCategories, setExpenseCategories] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(5); // You can change this value for page size customization
  const [languageData, setLanguageData] = useState([]);
  const [successMessage, setSuccessMessage] = useState("");
  const [languageInputs, setLanguageInputs] = useState<Record<number, string>>(
    {}
  );
  useEffect(() => {
    fetchExpenseData();
  }, []);

  const fetchExpenseData = async () => {
    try {
      const languages = await getLanguageType();
      const data = await getExpenseCategory();
      setLanguageData(languages);
      setExpenseCategories(data);
    } catch (error) {
      console.error("Error fetching expense categories:", error);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setExpenseCategoryData((prevData) => ({
      ...prevData,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleLanguageInputChange = (e: React.ChangeEvent<HTMLInputElement>, languageTypeId: number) => {
    const { value } = e.target;
    setLanguageInputs((prev) => ({
      ...prev,
      [languageTypeId]: value,
    }));
  };

  const handleGenerateJSON = () => {
    
    const jsonData = languageData.map((language: any) => ({
      languageTypeId: language.languageTypeId,
      languageName: language.languageName,
      TranslatedLanguage: languageInputs[language.languageTypeId] || "",
    }));
    return JSON.stringify(jsonData);
  };

  const handlePaginationChange = (newPage: number) => {
    setCurrentPage(newPage);
  };
  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    
    e.preventDefault();
    const incompleteFields = languageData.some(
      (language: any) =>
        !languageInputs[language.languageTypeId]?.trim()
    );

    if (incompleteFields) {
      const confirmFillDefault = window.confirm(
        `Some language fields are empty. If you proceed, the default language value will be the "${expenseCategoryData.name}". Do you want to continue?`
      );

      if (confirmFillDefault) {
        const updatedLanguageInputs = { ...languageInputs };
        languageData.forEach((language: any) => {
          if (!updatedLanguageInputs[language.languageTypeId]?.trim()) {
            updatedLanguageInputs[language.languageTypeId] =
              expenseCategoryData.name;
          }
        });
        setLanguageInputs(updatedLanguageInputs);
        return;
      } else {
        return;
      }
    }
    try {
      const languageJson = handleGenerateJSON();
      const updatedData = { ...expenseCategoryData, expenseCategoryLanguageJson: languageJson };

      if (isEditMode) {
        await updateExpenseCategory(updatedData);
      } else {
        await addExpenseCategory(updatedData);
      }
      setSuccessMessage(isEditMode ? "Expense Category Management updated successfully!" : "Expense Category Management added successfully!");

      setExpenseCategoryData({
        expenseCategoryID: 0,
        name: "",
        RoleId: 1,
        description: "",
        isActive: true,
        expenseCategoryLanguageJson: "",
      });
      setLanguageInputs({});
      setIsEditMode(false);
      fetchExpenseData();
      setTimeout(() => setSuccessMessage(""), 3000);

    } catch (error) {
      console.error("Error submitting data:", error);
    }
  };
  const handleRadioChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setExpenseCategoryData((prevData) => ({
        ...prevData,
        RoleId: parseInt(e.target.value, 10),
    }));
};

  const handleEdit = (category: any) => {debugger
    
    // Set the expense category data for editing
    setExpenseCategoryData({
      expenseCategoryID: category.expenseCategoryID,
      RoleId: category.roleId,
      name: category.name || "",
      description: category.description || "",
      isActive: category.isActive || true,
      expenseCategoryLanguageJson: category.expenseCategoryLanguageJson || "",
    });
    setIsEditMode(true);

    // Parse the JSON and update language inputs
    try {
      const parsedJson = JSON.parse(category.expenseCategoryLanguageJson);

      if (Array.isArray(parsedJson)) {
        // Map the parsed JSON to languageInputs
        const updatedLanguageInputs = parsedJson.reduce((acc: any, language: any) => {
          acc[language.languageTypeId] = language.TranslatedLanguage; // Assuming field name is 'translatedLanguage'
          return acc;
        }, {});           
        setLanguageInputs(updatedLanguageInputs);
      }
    } catch (error) {
      console.error("Error parsing JSON description:", error);
      setLanguageInputs({});
    }
  };


  const handleCancel = () => {
    setExpenseCategoryData({
      expenseCategoryID: 0,
      name: "",
      RoleId: 1,
      description: "",
      isActive: true,
      expenseCategoryLanguageJson: "",
    });
    setLanguageInputs({});
    setIsEditMode(false);
  };

  const totalPages = Math.ceil(expenseCategories.length / pageSize);
  const currentPageData = expenseCategories.slice((currentPage - 1) * pageSize, currentPage * pageSize);


  return (
    <div className="mt-4 ml-4">
      <h3>Expense Category Management</h3>
      <Form onSubmit={handleSubmit}>
        <Row>
          <Col xs={12} md={6}>
            <FormGroup>
              <Label for="name">Name</Label>
              <Input
                type="text"
                name="name"
                id="name"
                placeholder="Enter name"
                value={expenseCategoryData.name}
                onChange={handleChange}
                required
              />
            </FormGroup>
          </Col>
          <Col xs={12} md={6}>
            <FormGroup>
              <Label for="description">Description</Label>
              <Input
                type="text"
                name="description"
                id="description"
                placeholder="Enter description"
                value={expenseCategoryData.description}
                onChange={handleChange}
                required
              />
            </FormGroup>
          </Col>
          <Col xs={12} md={6}>
            <FormGroup>
              <Label>Is Active</Label>
              <Switch
                checked={expenseCategoryData.isActive}
                name="isActive"
                color="primary"
                onChange={handleChange}
              />
            </FormGroup>
          </Col>
          <Col md={6}>
                        <FormGroup>
                            <Label>Role Type</Label>
                            <div>
                                <Label check>
                                    <Input
                                        type="radio"
                                        name="RoleId"
                                        value="1"
                                        checked={expenseCategoryData.RoleId === 1}
                                        onChange={handleRadioChange}
                                    />

                                    Owner
                                </Label>
                                <Label check className="ms-3">
                                    <Input
                                        type="radio"
                                        name="RoleId"
                                        value="2"
                                        checked={expenseCategoryData.RoleId === 2}
                                        onChange={handleRadioChange}
                                    />
                                    Driver
                                </Label>
                            </div>
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

        <Button type="submit" color="primary" style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
        >
          {isEditMode ? "Update" : "Submit"}
        </Button>
        {isEditMode && (
          <Button color="secondary" onClick={handleCancel} className="ms-2" style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
>
            Cancel
          </Button>
        )}
      </Form>

      <h4>Expense Category Data</h4>
      <table className="table table-bordered">
        <thead>
          <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Role Type</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {currentPageData.map((category: any) => (
            <tr key={category.expenseCategoryID}>
              <td>{category.name}</td>
              <td>{category.description}</td>
              <td>{category.roleId === 1 ? "Owner" : "Driver"}</td>
              <td>
                <Button size="sm" style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
                  onClick={() => handleEdit(category)}>
                  Edit
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <div className="d-flex justify-content-between">
      <div className="pagination">
                            <Button
                                disabled={currentPage === 1}
                                onClick={() => handlePaginationChange(currentPage - 1)}
                                style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}

                            >
                                Previous
                            </Button>
                            <span>{`Page ${currentPage} of ${totalPages}`}</span>
                            <Button
                                disabled={currentPage === totalPages}
                                onClick={() => handlePaginationChange(currentPage + 1)}
                                style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}

                            >
                                Next
                            </Button>
                        </div>

        <style jsx>{`
                .form-container {
                    max-width: 100%;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #f8f9fa;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }
                .expense-category-form {
                    display: flex;
                    flex-direction: column;
                    gap: 20px;
                }
                .expense-category-list {
                    margin-top: 20px;
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }
              .submit-button {
        margin-top: 20px;
        width: 100%;
        max-width: 200px;
        align-self: center;
        background-color: #ff0000; /* Red background */
        border-color: #ff0000;    /* Red border */
        color: white;             /* White text */
        transition: background-color 0.3s, border-color 0.3s;
    }
    .submit-button:hover {
        background-color: #cc0000; /* Slightly darker red on hover */
        border-color: #cc0000;    /* Match hover border color */
    }
                .table {
                    width: 100%;
                    border-collapse: collapse;
                }
                .table th, .table td {
                    text-align: left;
                    padding: 8px;
                }
                .table th {
                    background-color: #f2f2f2;
                }
                @media (max-width: 768px) {
                    .submit-button {
                        width: 100%;
                    }
                    .expense-category-form {
                        flex-direction: column;
                    }
                    .expense-category-list {
                        padding: 15px;
                    }
                }
            `}</style>
      </div>

    </div>
  );
};

export default Page;
