import React from 'react'

const page = ({params}:any) => {
    const {id} = params
  return (
    <div>Edit.{id}</div>
  )
}

export default page