const getFileFormat = (file: any) => {
  const parts = file.type.split("/");
  const fileType = parts[1]; // This will be 'jpeg', 'png', etc.

  // Concatenate '.' with fileType and return
  return `.${fileType}`;
};

export default getFileFormat;
